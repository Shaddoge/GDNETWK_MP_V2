using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        // Send received packet
        ClientSend.WelcomeReceived();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        Debug.Log("SPAWN PLAYER");
        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        if(GameManager.players.Count == 0) return;
        //if (!Client.instance.IsConnected) return;
        int _id = _packet.ReadInt();
        Vector3 _newPosition = _packet.ReadVector3();
        //Debug.Log(GameManager.players.Count);

        GameManager.players[_id].LerpPos(_newPosition);
        //GameManager.players[_id].transform.position = _newPosition;
    }

    public static void PlayerRotation(Packet _packet)
    {
        if(GameManager.players.Count == 0) return;
        //if (!Client.instance.IsConnected) return;
        int _id = _packet.ReadInt();
        Quaternion _newRotation = _packet.ReadQuaternion();

        GameManager.players[_id].LerpRot(_newRotation);
        //GameManager.players[_id].transform.rotation = _newRotation;
    }

    public static void PlayerDisconnected(Packet  _packet)
    {
        int _id = _packet.ReadInt();
        
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }
}
