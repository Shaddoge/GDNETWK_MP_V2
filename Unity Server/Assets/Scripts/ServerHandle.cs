using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString(); 

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if(_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!"); 
        }
        Server.clients[_fromClient].SendIntoGame(_username);
        // Send player into game
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool [_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Server.clients[_fromClient].player.SetInput(_inputs);
    }

    public static void PlayerReady(int _fromClient, Packet _packet)
    {
        bool _isReady = _packet.ReadBool();

        Server.clients[_fromClient].player.SetReady(_isReady);
        ServerSend.PlayerReady(_fromClient, _isReady);

        GameManager.instance.ReadyCheck();
    }

    public static void PlayerSendChat(int _fromClient, Packet _packet)
    {
        string _message = _packet.ReadString();

        string _display = Server.clients[_fromClient].player.username + ": " + _message;
        Debug.Log(_display);
        ServerSend.PlayerChat(_display);
    }
}
