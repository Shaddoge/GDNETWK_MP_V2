using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private InputField[] ipFields;
    [SerializeField] private InputField portField;
    public InputField usernameField;
    [SerializeField] private GameObject LobbyPanel;

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

        foreach(InputField input in ipFields)
        {
            input.interactable = false;
        }

        portField.interactable = false;
        usernameField.interactable = false;
        Client.instance.ConnectToServer(GetIpValue(), (portField.text != "") ? int.Parse(portField.text) : int.Parse(portField.placeholder.GetComponent<Text>().text));
        if(LobbyPanel != null)
        {
            LobbyPanel.SetActive(true);
        }   
    }

    private string GetIpValue()
    {
        string _ip = "";

        for (int i = 0; i < ipFields.Length; i++)
        {
            
            if (ipFields[i].text != "")
            {
                _ip += ipFields[i].text;
            }
            else
            {
                _ip += ipFields[i].placeholder.GetComponent<Text>().text;
            }
            
            _ip += (i != ipFields.Length - 1) ? "." : "";
        }

        return _ip;
    }
}
