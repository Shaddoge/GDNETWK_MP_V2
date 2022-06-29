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
        UIManager.instance.ChatboxToggle(true);
        // Send received packet
        ClientSend.WelcomeReceived();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        int _trackId = _packet.ReadInt();

        GameManager.instance.ChangeTrack(_trackId);
        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        UIManager.instance.ToggleLobby(true);
    }

    public static void PlayerMovement(Packet _packet)
    {
        if (GameManager.players.Count == 0) return;

        int _id = _packet.ReadInt();    
        Vector3 _newPosition = _packet.ReadVector3();
        Quaternion _newRotation = _packet.ReadQuaternion();
        List<Vector3> wheelPos = new List<Vector3>();
        List<Quaternion> wheelRot = new List<Quaternion>();

        if (GameManager.players[_id] == null) return;

        for(int i = 0; i < 4; i++)
        {
            Vector3 pos = _packet.ReadVector3();
            Quaternion rot = _packet.ReadQuaternion();
            wheelPos.Add(pos);
            wheelRot.Add(rot);
        }

        GameManager.players[_id].LerpPos(_newPosition);
        GameManager.players[_id].LerpRot(_newRotation);
        GameManager.players[_id].LerpWheels(wheelPos, wheelRot);
    }

    public static void PlayerState(Packet _packet)
    {
        if (GameManager.players.Count == 0) return;
        //if (!Client.instance.IsConnected) return;
        int _id = _packet.ReadInt();
        bool _isReady = _packet.ReadBool();

        if (GameManager.players[_id] == null) return;
        GameManager.players[_id].isReady = _isReady;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        
        if(_id != Client.instance.myId)
            UIManager.instance.CreateFeed($"{GameManager.players[_id].username} disconnected.");

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
        ProfileManager.instance.RemoveProfile(_id);
    }

    public static void PositionChanged(Packet _packet)
    {
        int _place = _packet.ReadInt();

        UIManager.instance.ChangeMyPosition(_place);
    }

    public static void PlayerFinished(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _place = _packet.ReadInt();
        float _time = _packet.ReadFloat();

        if(_id == Client.instance.myId)
        {
            UIManager.instance.EndTimerHide();
            UIManager.instance.GameOver(_place, _time);
        }
        else
        {
            UIManager.instance.CreateFinishFeed(_id, _place);
        }
    }

    // Player did not finish
    public static void PlayerDNF(Packet _packet)
    {
        int _id = _packet.ReadInt();

        if(_id != Client.instance.myId)
        {
            UIManager.instance.CreateDNFFeed(_id);
        }
        else
        {
            UIManager.instance.EndTimerHide();
        }
    }

    public static void PlayerChat(Packet _packet)
    {
        string _message = _packet.ReadString();
        UIManager.instance.AddChatInstance(_message);
    }

    public static void PlayerReady(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isReady = _packet.ReadBool();
        ProfileManager.instance.ToggleCheck(_id, _isReady);
        //ProfileManager.instance.
    }

    public static void GameState(Packet _packet)
    {
        int _idState = _packet.ReadInt();

        switch(_idState)
        {
            case 0: ProfileManager.instance.TimerStarted();
                    UIManager.instance.StartTimerStarted();
                    GameManager.instance.ToggleAllTireFX(true); break;
            case 1: UIManager.instance.EndTimerStarted(); break; // Display Timer
            case 2: UIManager.instance.NewTrack(); break; // Reset Lobby
        }
        Debug.Log(_idState);
    }

    public static void TrackChange(Packet _packet)
    {
        int _trackId = _packet.ReadInt();

        GameManager.instance.ChangeTrack(_trackId);
    }
}
