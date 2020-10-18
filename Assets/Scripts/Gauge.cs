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

    public void setGauge(float value)
    {
        slider.value += value;
    }
}
