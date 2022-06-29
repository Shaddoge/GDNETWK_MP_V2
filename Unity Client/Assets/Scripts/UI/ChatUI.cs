using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private GameObject chatInstance;
    [SerializeField] private InputField chatInput;
    [SerializeField] private Transform chatContent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (EventSystem.current.currentSelectedGameObject == chatInput.gameObject)
            {
                SendChat();
            }
            else
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
        GameObject newChatInstance = Instantiate(chatInstance, chatContent);
        newChatInstance.GetComponent<Text>().text = _message;
    }
}
