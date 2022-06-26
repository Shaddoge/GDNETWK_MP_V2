using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool gameStarted = false;

    [HideInInspector] public float trackTime = 0f;

    [SerializeField] private GameObject[] tracks;
    private int currentTrack = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of GameManager instance");
            Destroy(this);
        }
    }

    private void Update()
    {
        if (gameStarted)
        {
            trackTime += Time.deltaTime;
        }
    }

    public void ReadyCheck()
    {
        int totalPlayers = 0;
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                totalPlayers++;
                if(!Server.clients[i].player.isReady)
                {
                    return;
                }
            }
        }

        if (totalPlayers == 0) return;

        
        StartCoroutine(GamePrepare());
    }

    public IEnumerator GamePrepare()
    {
        ServerSend.GameState(0);
        yield return new WaitForSeconds(5f);

        ToggleAllPlayerMove(true);

        gameStarted = true;
    }

    public IEnumerator FinishCountdown()
    {
        ServerSend.GameState(1); // Display Timer
        yield return new WaitForSeconds(10f);
        // Game end;
        Debug.Log("GAME END");
        ToggleAllPlayerMove(false);
        gameStarted = false;
        // Should send end screen

        ResetAllPlayerReady();
        yield return new WaitForSeconds(5f);
        
        NextTrack();
    }

    private void NextTrack()
    {
        ServerSend.GameState(2);
        currentTrack = (currentTrack + 1) % tracks.Length;

        for (int i = 0; i < tracks.Length; i++)
        {
            if(i == currentTrack)
            {
                tracks[i].SetActive(true);
            }
            else
            {
                tracks[i].SetActive(false);
            }
        }
        
        // Send info to client and change level
        ServerSend.TrackChange(currentTrack);
        // Reposition the players
        // Ready check
    }

    public void ToggleAllPlayerMove(bool _canMove)
    {
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                Server.clients[i].player.canMove = _canMove;
            }
        }
    }

    public void ResetAllPlayerReady()
    {
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                Server.clients[i].player.isReady = false;
                ServerSend.PlayerReady(i, false);
            }
        }
    }
}
