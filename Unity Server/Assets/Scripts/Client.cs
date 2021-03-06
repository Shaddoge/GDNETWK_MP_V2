using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public static int dataBufferSize = 4096;
    public int id;
    public Player player;
    public TCP tcp;
    public Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
    }

    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            // Send packet
            ServerSend.Welcome(id, "You have successfully joined the server.");
        } 

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                    
                // Disconnect
                if(_byteLength <= 0) 
                {
                    Server.clients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                // Handle Data
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            // Disconnect
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP Data: {_ex}");
                Server.clients[id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if(_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if(_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            
            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public void SendIntoGame(string _playerName)
    {
        player = NetworkManager.instance.InstantiatePlayer(id);
        player.Initialize(id, _playerName);
        Debug.Log($"{_playerName} joined the game.");
        foreach (Client _client in Server.clients.Values)
        {
            // Make sure player is not null
            if (_client.player == null) continue;
            // Send data of players beside the current player
            if (_client.id == id) continue;

            ServerSend.SpawnPlayer(id, _client.player);
        }

        foreach (Client _client in Server.clients.Values)
        {
            // Make sure player is not null
            if (_client.player == null) continue;
            ServerSend.SpawnPlayer(_client.id, player);
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            if (player.gameObject)
                UnityEngine.Object.Destroy(player.gameObject);
            player = null;
            
            if(Server.GetNumPlayers() == 0)
            {
                GameManager.instance.ResetTrack();
            }
        });

        tcp.Disconnect();

        ServerSend.PlayerDisconnected(id);
    }
}
