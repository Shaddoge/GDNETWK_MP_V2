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
    [SerializeField] private EndTimerUI endTimerUI;

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
    }

    public string GetUsername()
    {
        return connectUI.usernameField.text;
    }

    #region End/Restart
    public void EndTimerStarted()
    {
        if (gameOverUI.IsDisplayed) return;
        endTimerUI.TimerStart();
    }

    public void EndTimerHide()
    {
        endTimerUI.TimerHide();
    }

    public void GameOver(int _place, float _time)
    {
        posUI.gameObject.SetActive(false);
        gameOverUI.GameOverDisplay(_place, _time);
    }

    public void NewTrack()
    {
        gameOverUI.CloseGameOver();
        StartCoroutine(DelayActiveUI());
    }

    private IEnumerator DelayActiveUI()
    {
        yield return new WaitForSeconds(1f);
        ProfileManager.instance.ResetAll();
        ToggleLobby(true);
        posUI.gameObject.SetActive(true);
    }
    #endregion

    #region Lobby
    public void ToggleLobby(bool _isActive)
    {
        if(_isActive)
            lobbyPanel.SetActive(true);
        lobbyPanel.GetComponent<Animator>().SetBool("IsShow", _isActive);
    }

    public void StartTimerStarted()
    {
        lobbyPanel.GetComponent<StartGameTimer>().StartTimer();
        lobbyPanel.GetComponent<Animator>().SetBool("IsShow", false);
        ProfileManager.instance.CloseAnimation();
    }

    #endregion

    #region Feed
    public void CreateFeed(string _text)
    {
        feedUI.CreateFeed(_text);
    }

    public void CreateFinishFeed(int _id, int _place)
    {
        string _displayPlace = _place.ToString();
        switch(_place)
        {
            case 1: _displayPlace += "st"; break;
            case 2: _displayPlace += "nd"; break;
            default: _displayPlace += "th"; break;
        }

        UIManager.instance.CreateFeed($"{GameManager.players[_id].username} finished in {_displayPlace}!");
    }

    public void CreateDNFFeed(int _id)
    {
        UIManager.instance.CreateFeed($"{GameManager.players[_id].username} did not finish!");
    }

    #endregion

    #region Chat
    public void AddChatInstance(string _message)
    {
        chatUI.AddChatInstance(_message);
    }

    public void ChatboxToggle(bool _isActive)
    {
        chatUI.gameObject.SetActive(_isActive);
    }
    #endregion

    #region Position
    public void ChangeMyPosition(int _place)
    {
        posUI.SetPosition(_place);
    }
    #endregion

}
