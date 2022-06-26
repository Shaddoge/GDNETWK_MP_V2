using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
        
    public static void Start(int _maxPlayers, int _port)
    {
        MaxPlayers = _maxPlayers;
        Port = _port;

        Console.WriteLine("Server booting up...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        Console.WriteLine($"Server started on {Port}.");
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if(clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);
                return;
            }
        }

        Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full");
    }
    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
            { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
            { (int)ClientPackets.playerReady, ServerHandle.PlayerReady },
            { (int)ClientPackets.playerSendChat, ServerHandle.PlayerSendChat },
        };
        Console.WriteLine("Initialized packets.");
    }

    public static void ReadyCheck()
    {
        int totalPlayers = 0;
        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket != null)
            {
                totalPlayers++;
                if(!clients[i].player.isReady)
                {
                    return;
                }
            }
        }

        if (totalPlayers == 0) return;

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket != null)
            {
                clients[i].player.StartTimer();
            }
        }
    }

    public static void Stop()
    {
        tcpListener.Stop();
    }
}
