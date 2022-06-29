using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private bool startMovement = false;
    private CarSFX carSFX;

    private void Start()
    {
        carSFX = this.GetComponent<CarSFX>();
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

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

        if (!SoundManager.instance.inCountDownCarSFX)
        {
            if (CheckIfInput(_inputs))
            {
                GenerateSFX(_inputs);
            }
            else if (carSFX.IsPlaying())
            {
                carSFX.Stop();
            }
        }
        ClientSend.PlayerMovement(_inputs);
    }

    private void GenerateSFX(bool[] inputs)
    {
        // if moving forward or backward
        if (inputs[0] || inputs[1])
        {
            carSFX.PlayCarDrive();
        }

        if (inputs[4])
        {
            carSFX.PlayCarBrake();
        }
    }

    private bool CheckIfInput(bool[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i])
                return true;
        }
        return false;
    }
}
