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
        gameObject.GetComponent<Text>().text = description;
    }
}
