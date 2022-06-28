using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTimerUI : MonoBehaviour
{
    private float currentTime = 0f;
    private float startingTime = 20f;
    private bool timeRunning = false;

    [SerializeField] private Text countdownText;
    [SerializeField] private Image countdownProgressBar;

    private void Update()
    {
        if (timeRunning && currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;

            if(countdownText.text != currentTime.ToString("0"))
                countdownText.text = currentTime.ToString("0");

            if (currentTime < 5 && countdownProgressBar.color != Color.red)
            {
                countdownProgressBar.color = Color.red;
            }
            else if (currentTime < 10 && countdownProgressBar.color != new Color32(255, 135, 0, 1))
            {
                countdownProgressBar.color = new Color32(255, 135, 0, 1);
            }

            if (currentTime <= 0)
            {
                currentTime = startingTime;
                countdownText.text = "DNF";
                TimerHide();
            }

            float time = currentTime / startingTime;

            countdownProgressBar.fillAmount = time;
        }
    }

    public void TimerStart()
    {
        this.GetComponent<Animator>().SetBool("IsShow", true);
        timeRunning = true;
    }

    public void TimerHide()
    {
        this.GetComponent<Animator>().SetBool("IsShow", false);
        timeRunning = false;
    }
}
