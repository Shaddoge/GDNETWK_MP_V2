using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    [SerializeField] private GameObject chatInstance;
    [SerializeField] private InputField chatInput;
    [SerializeField] private Transform chatContent;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of ProfileManager instance");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(chatInput.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SendChat();
            }
        }
        else
        {
            if (chatInput.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatInput.ActivateInputField();
            }
        }
    }

    public void SendChat()
    {
        if (chatInput.text == "") {
            chatInput.DeactivateInputField();
            return;
        }

        ClientSend.PlayerSendChat(chatInput.text);
        chatInput.text = "";
    }

    public void AddChatInstance(string _message)
    {
        Instantiate(chatInstance, chatContent);
        chatInstance.GetComponent<Text>().text = _message;
    }
}
