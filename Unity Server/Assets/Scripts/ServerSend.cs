using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    #region Packets
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerWheels(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerWheels))
        {
            _packet.Write(_player.id);
            foreach(WheelCollider wheel in _player.wheelColliders)
            {
                Vector3 pos;
                Quaternion rot;

                wheel.GetWorldPose(out pos, out rot);
                _packet.Write(pos);
                _packet.Write(rot);
            }
            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerState(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerState))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.isReady);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PositionChanged(int _playerId, int _place)
    {
        using (Packet _packet = new Packet((int)ServerPackets.positionChanged))
        {
            _packet.Write(_place);

            SendTCPData(_playerId, _packet);
        }
    }

    public static void PlayerFinished(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerFinished))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerChat(string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerChat))
        {
            _packet.Write(_message);
            Debug.Log($"Sending: {_message}");
            SendTCPDataToAll(_packet);
        }
    }

    #endregion
}
