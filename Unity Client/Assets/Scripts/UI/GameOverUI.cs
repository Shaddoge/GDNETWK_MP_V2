using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    
    [SerializeField] private GameObject popup;
    [SerializeField] private Text headerText;
    [SerializeField] private Text timeText;

    private Animator animator;
    private bool isDisplayed = false;
    public bool IsDisplayed { get { return isDisplayed; } }

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void GameOverDisplay(int _place, float _time)
    {
        string _displayPlace = _place.ToString();

        if(_place > 0)
        {
            switch(_place)
            {
                case 1: _displayPlace += "st"; break;
                case 2: _displayPlace += "nd"; break;
                default: _displayPlace += "th"; break;
            }
            headerText.text = $"You finished in {_displayPlace} place!" ;
        }
        else
        {
            headerText.text = "You did not finish the race!";
        }

        isDisplayed = true;
        
        timeText.text = $"{_time.ToString("F2")}s";
        animator.SetTrigger("Finish");
    }

    public void CloseGameOver()
    {
        isDisplayed = false;
        animator.SetTrigger("Close");
    }
}
