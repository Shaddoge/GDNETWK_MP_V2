using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Text headerText;
    [SerializeField] private Text timeText;

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
        popup.SetActive(true);
    }
}
