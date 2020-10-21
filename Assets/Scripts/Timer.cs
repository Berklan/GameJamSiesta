using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    private float currentTime;
    private bool overrideTimer = false;

    private Text timerText;

    private void Awake()
    {
        currentTime = timeLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        timerText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        timerText.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");

        if (currentTime < 10f)
            timerText.color = Color.red;

        if(currentTime <= 0f)
        {
            Debug.Log("Game Over");
        }
    }

    public void SetNewTimer(float time)
    {
        currentTime = time;
        overrideTimer = true;
        timerText.enabled = true;
    }

    public float GetTimer()
    {
        return currentTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!overrideTimer)
        {
            Debug.Log(currentTime);
            timerText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!overrideTimer)
            timerText.enabled = false;
    }
}
