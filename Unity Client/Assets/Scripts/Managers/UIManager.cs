using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Panels")]
    [SerializeField] private ConnectUI connectUI;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameOverUI gameOverUI;
    

    [Header("HUD")]
    [SerializeField] private ChatUI chatUI;
    [SerializeField] private FeedUI feedUI;
    [SerializeField] private PositionUI posUI;
    

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
        connectUI.ConnectToServer();

        if(lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }
    }

    public string GetUsername()
    {
        return connectUI.usernameField.text;
    }

    public void StartTimerStarted()
    {
        lobbyPanel.GetComponent<StartGameTimer>().StartTimer();
    }

    public void GameOver(int _place, float _time)
    {
        gameOverUI.GameOverDisplay(_place, _time);
    }

    public void CreateFeed(string _text)
    {
        feedUI.CreateFeed(_text);
    }

    public void AddChatInstance(string _message)
    {
        chatUI.AddChatInstance(_message);
    }

    public void ChangePositionDisplay(int _place)
    {
        posUI.SetPosition(_place);
    }

    public void ChatboxToggle(bool _isActive)
    {
        chatUI.gameObject.SetActive(_isActive);
    }
}
