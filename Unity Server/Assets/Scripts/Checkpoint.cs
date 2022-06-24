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
            if (nextTarget != null)
            {
                // If trigger is the correct next checkpoint
                if (id >= other.GetComponent<Player>().nextCheckpoint.id)
                {
                    other.GetComponent<Player>().nextCheckpoint = nextTarget;
                    CheckpointHandler.CalculatePlacement();
                }
            }
            else
            {
                // Finish line
                // Send player id
                Debug.Log("FINISH LINE!");
                ServerSend.PlayerFinished(other.GetComponent<Player>().id);
            }
        }
    }
}
