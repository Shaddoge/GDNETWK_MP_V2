using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] private static List<Checkpoint> checkpoints = new List<Checkpoint>();

    void Awake()
    {
        if (checkpoints.Count == 0)
        {
            Checkpoint[] children = transform.GetComponentsInChildren<Checkpoint>();
            for(int i = 0; i < children.Length; i++)
            {
                children[i].id = i;

                if(i + 1 < children.Length)
                {
                    children[i].nextTarget = children[i + 1];
                }
                checkpoints.Add(children[i]);
            }
        }
    }

    public static void CalculatePlacement()
    {
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if(Server.clients[i].player == null) continue;
            int newPlacement = 1;

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
        
        
        /*for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if(Server.clients[i].player != null)
            {
                Server.clients[i].player.placement = i;
                
            }
        };*/
    }

    public static Checkpoint GetFirstCheckpoint()
    {
        if(checkpoints.Count != 0)
            return checkpoints[0];
        else
            return null;
    }
}
