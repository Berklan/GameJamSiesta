using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    public string shortDescription;
    public string description;
    public Actions actionNeeded; 
    public bool condition;


    private void Start()
    {
        condition = false;
    }
}
