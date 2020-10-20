using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    private float currentTime;

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

        if(currentTime <= 0f)
        {
            Debug.Log("Game Over");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(currentTime);
        timerText.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        timerText.enabled = false;
    }
}
