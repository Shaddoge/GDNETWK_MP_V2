using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int id;
    public Checkpoint nextTarget;

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
                if(player.isFinished) return;

                CheckpointHandler.instance.numFinished++;
                if (CheckpointHandler.instance.numFinished == 1)
                {
                    StartCoroutine(GameManager.instance.FinishCountdown());
                }
                player.canMove = false;
                ServerSend.PlayerFinished(player.id, CheckpointHandler.instance.numFinished, GameManager.instance.trackTime);
            }
        }
    }
}
