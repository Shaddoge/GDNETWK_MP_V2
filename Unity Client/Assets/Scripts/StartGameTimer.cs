using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class StartGameTimer : MonoBehaviour
{
    private float currentTime = 0f;
    private float startingTime = 5f;
    private bool startTime = false;

    [SerializeField] private Text countdownText;
    [SerializeField] private Image countdownProgressBar;
    [SerializeField] private GameObject TimerUI;

    void Start()
    {
        currentTime = startingTime;
        countdownProgressBar.color = Color.red;
    }

    void Update()
    {
        if (GameManager.instance.startGame)
        {
            startTime = true;
            TimerUI.SetActive(true);
        }

        if (startTime)
        {
            currentTime -= 1 * Time.deltaTime;

            countdownText.text = currentTime.ToString("0");

            if (currentTime < 1)
            {
                countdownProgressBar.color = Color.green;
                countdownText.text = "START";
            }

            if (currentTime <= 0)
            {
                currentTime = startingTime;
                startTime = false;
                this.gameObject.SetActive(false);
            }

            float time = currentTime / startingTime;

            countdownProgressBar.fillAmount = time;
        }
    }

    public void StartTimer()
    {
        startTime = true;
    }
}
