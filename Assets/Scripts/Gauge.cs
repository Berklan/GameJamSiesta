using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{

    private Slider slider;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = 0;
    }

    public void Update()
    {
        if(slider.value >= slider.maxValue)
            GameObject.Find("SceneController").GetComponent<SceneController>().GameOverMessage("awake");
    }

    public void setGauge(float value)
    {
        slider.value += value;
    }
}
