using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectUI : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private InputField[] ipFields;
    [SerializeField] private InputField portField;
    public InputField usernameField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ConnectToServer()
    {
        gameObject.SetActive(false);

        foreach(InputField input in ipFields)
        {
            input.interactable = false;
        }

        portField.interactable = false;
        usernameField.interactable = false;
        Client.instance.ConnectToServer(GetIpValue(), (portField.text != "") ? int.Parse(portField.text) : int.Parse(portField.placeholder.GetComponent<Text>().text));
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
