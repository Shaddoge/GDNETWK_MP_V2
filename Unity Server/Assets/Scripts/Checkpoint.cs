using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int id;
    public Checkpoint nextTarget;
    private Coroutine waitCountdown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Has next checkpoint
            if (nextTarget != null)
            {
                // If trigger is the correct next checkpoint
                if (id >= other.GetComponent<Player>().nextCheckpoint.id)
                {
                    other.GetComponent<Player>().nextCheckpoint = nextTarget;
                    CheckpointHandler.instance.CalculatePlacement();
                }
            }
            // Finish line
            else
            {
                Player player = other.GetComponent<Player>();
                if (player.isFinished) return;

                player.isFinished = true;
                player.canMove = false;
                CheckpointHandler.instance.numFinished++;

                if(CheckpointHandler.instance.numFinished >= Server.GetNumPlayers())
                {
                    StartCoroutine(GameManager.instance.AllFinished());

                    // Cancel wait countdown if everyone is finished.
                    if (waitCountdown != null)
                    {
                        StopCoroutine(waitCountdown);
                        waitCountdown = null;
                    }
                }
                else if (CheckpointHandler.instance.numFinished == 1 && waitCountdown == null)
                {
                    waitCountdown = StartCoroutine(GameManager.instance.FinishCountdown());
                }
                
                ServerSend.PlayerFinished(player.id, CheckpointHandler.instance.numFinished, GameManager.instance.trackTime);
            }
        }
    }
}
