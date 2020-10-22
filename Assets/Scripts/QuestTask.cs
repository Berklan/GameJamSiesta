using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTask : MonoBehaviour
{
    public string shortDescription;
    public string description;
    public Actions actionNeeded; 
    public bool condition;


    private void Start()
    {
        condition = false;
        Text text = gameObject.GetComponent<Text>();
        text.text = description;
        text.fontSize = 14;
    }
}
