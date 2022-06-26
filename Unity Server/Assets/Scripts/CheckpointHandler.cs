using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    public static CheckpointHandler instance;
    [SerializeField] private List<GameObject> tracks = new List<GameObject>();
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    public int numFinished = 0;
    private int trackSelected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of CheckpointHandler instance");
            Destroy(this);
        }
    }

    private void Start()
    {
        numFinished = 0;
        trackSelected = 0;

        // Initialize checkpoints
        foreach(GameObject track in tracks)
        {
            Checkpoint[] children = track.transform.GetComponentsInChildren<Checkpoint>();
            for(int i = 0; i < children.Length; i++)
            {
                children[i].id = i;

                if(i + 1 < children.Length)
                {
                    children[i].nextTarget = children[i + 1];
                }
            }
        }

        ChangeCheckpointFromTrack(0);
    }

    public void CalculatePlacement()
    {
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if(Server.clients[i].player == null) continue;
            int newPlacement = 1 + numFinished;

            for (int j = 1; j <= Server.MaxPlayers; j++)
            {
                if(Server.clients[j].player == null) continue;
                if(Server.clients[j].player == Server.clients[i].player) continue;
                
                Checkpoint nextCp1 = Server.clients[i].player.nextCheckpoint;
                Checkpoint nextCp2 = Server.clients[j].player.nextCheckpoint;

                if(nextCp1.id < nextCp2.id)
                {
                    newPlacement++;
                }
                else if(nextCp1.id == nextCp2.id)
                {
                    // Calculate distance
                    float distance1 = Vector3.Distance(Server.clients[i].player.transform.position, nextCp1.transform.position);
                    float distance2 = Vector3.Distance(Server.clients[j].player.transform.position, nextCp2.transform.position);

                    if (distance1 > distance2)
                    {
                        newPlacement++;
                    }
                    //Server.clients[i].player.gameObject;
                }
            }

            Server.clients[i].player.placement = newPlacement;
            Debug.Log($"Player {Server.clients[i].player.username}, Position: {newPlacement}");
            ServerSend.PositionChanged(i, newPlacement);
        };
    }

    public Checkpoint GetFirstCheckpoint()
    {
        return (checkpoints.Count != 0) ? checkpoints[0] : null;
    }

    public void ChangeCheckpointFromTrack(int _trackId)
    {
        checkpoints.Clear();
        numFinished = 0;

        for (int i = 0; i < tracks.Count; i++)
        {
            bool chosenTrack = (i == _trackId) ? true : false;
            tracks[i].SetActive(chosenTrack);
        }

        foreach(GameObject checkPoint in tracks[_trackId].transform)
        {
            if (checkPoint.GetComponent<Checkpoint>() != null)
                checkpoints.Add(checkPoint.GetComponent<Checkpoint>());
        }
    }
}
