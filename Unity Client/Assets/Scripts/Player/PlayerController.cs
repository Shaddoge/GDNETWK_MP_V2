using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private bool startMovement = false;

    private void FixedUpdate()
    {
        /*if (GameManager.instance.startGame)
        {
            StartCoroutine("StartGame");
        }*/

        SendInputToServer();

        /*if (startMovement)
        {
            
        }*/
    }

    /*IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        startMovement = true;
    }*/

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.UpArrow),
            Input.GetKey(KeyCode.DownArrow),
            Input.GetKey(KeyCode.LeftArrow),
            Input.GetKey(KeyCode.RightArrow),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }
}
