using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted = false;

    [HideInInspector] public float trackTime = 0f;
    private bool timeRunning = false;

    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject[] tracks;
    private int currentTrack = 0;
    public int CurrentTrack { get { return currentTrack; } }


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
        if (timeRunning)
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

    // This function should run when all players are ready
    public IEnumerator GamePrepare()
    {
        gameStarted = true;
        ServerSend.GameState(0);
        yield return new WaitForSeconds(5f);

        if(Server.GetNumPlayers() != 0)
        {
            ToggleAllPlayerMove(true);
            timeRunning = true;
        }
        else
        {
            gameStarted = false;
        }
    }

    // This function is used when someone reached the finish line
    public IEnumerator FinishCountdown()
    {
        ServerSend.GameState(1); // Display Timer
        yield return new WaitForSeconds(20f);

        // Game end
        timeRunning = false;
        ToggleAllPlayerMove(false);
        // Send DNF
        CheckPlayersDNF();
        
        StartCoroutine(AllFinished());
    }

    public IEnumerator AllFinished()
    {
        yield return new WaitForSeconds(5f);
        
        currentTrack++;
        ChangeTrack();
    }

    public void ResetTrack()
    {
        currentTrack = 0;
        ChangeTrack();
    }

    // This function will change to the next track and reposition the players
    private void ChangeTrack()
    {
        currentTrack = currentTrack % tracks.Length;

        CheckpointHandler.instance.ChangeCheckpointFromTrack(currentTrack);
        for (int i = 0; i < tracks.Length; i++)
        {
            bool chosen = (i == currentTrack) ? true : false;
            tracks[i].SetActive(chosen);
        }
        
        // Send info to client and change level
        ServerSend.TrackChange(currentTrack);
        
        // Reposition the players
        RepositionPlayers(currentTrack);

        // Reset values
        ResetPlayers();
        gameStarted = false;
        timeRunning = false;
        trackTime = 0f;

        // Ready check
        ServerSend.GameState(2);
    }

    public Vector3 GetCurrentSpawnPos(int _playerId)
    {
        return spawns[currentTrack].transform.GetChild(_playerId - 1).position;
    }

    public Quaternion GetCurrentSpawnRot(int _playerId)
    {
        return spawns[currentTrack].transform.GetChild(_playerId - 1).rotation;
    }

    public void RepositionPlayers(int _trackId)
    {
        if(_trackId >= spawns.Length || _trackId < 0) return;

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                int _playerId = Server.clients[i].player.id;
                Vector3 targetPos = spawns[_trackId].transform.GetChild(_playerId - 1).position;
                Quaternion targetRot = spawns[_trackId].transform.GetChild(_playerId - 1).rotation;

                Server.clients[i].player.transform.position = targetPos;
                Server.clients[i].player.transform.rotation = targetRot;
            }
        }
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

    public void CheckPlayersDNF()
    {
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                if (!Server.clients[i].player.isFinished)
                    ServerSend.PlayerDNF(Server.clients[i].player.id);
            }
        }
    }

    // Reset player states
    public void ResetPlayers()
    {
        ToggleAllPlayerMove(false);
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                // Reset Ready State
                Server.clients[i].player.ResetValues();
                //ServerSend.PlayerReady(i, false);
            }
        }
    }
}
