using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }
        
    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            string username = UIManager.instance.usernameField.text;
            _packet.Write(username != "" ? username : "Guest");

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }

            SendTCPData(_packet);
        }
    }

    public static void PlayerReady(bool _isReady)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerReady))
        {
            _packet.Write(_isReady);

            SendTCPData(_packet);
        }
    }

    public static void PlayerSendChat(string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerSendChat))
        {
            //_packet.Write(Client.instance.myId);
            _packet.Write(_message);

            SendTCPData(_packet);
        }
    }
    #endregion
}
