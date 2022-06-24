using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mainMenu;
    public InputField usernameField;
    public GameObject LobbyPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of UIManager instance");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        mainMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        LobbyPanel.SetActive(true);
    }

}
