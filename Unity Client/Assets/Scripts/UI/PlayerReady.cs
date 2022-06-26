using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReady : MonoBehaviour
{
    public Toggle isReady;

    public void Ready()
    {
        ClientSend.PlayerReady(isReady.isOn);
    }
}
