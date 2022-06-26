using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text headerText;
    [SerializeField] private Text timeText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Removing the copy of GameOverManager instance");
            Destroy(this);
        }
    }

    public void GameOverDisplay(int _place, float _time)
    {
        string _displayPlace = _place.ToString();
        switch(_place)
        {
            case 1: _displayPlace += "st"; break;
            case 2: _displayPlace += "nd"; break;
            default: _displayPlace += "th"; break;
        }

        headerText.text = $"You finished in {_displayPlace} place!" ;
        timeText.text = $"{_time.ToString("F2")}s";
        gameOverPanel.SetActive(true);
    }
}
