using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTab : MonoBehaviour
{
    public List<GameObject> quests;
    private int completedTasks;
    private Vector3 previousPos;
    private Quest quest;

    CanvasGroup canvas;

    private void Awake()
    {
        GetQuest();
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void OpenQuestTab()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    private void GetQuest()
    {
        completedTasks = 0;

        Vector3 parentPos = gameObject.transform.position;

        previousPos = new Vector3(parentPos.x - 20, parentPos.y - 60, 0);

        // Get a random quest
        int index = Random.Range(0, quests.Count);
        quest = quests[index].GetComponent<Quest>();

        foreach(GameObject task in quest.tasks)
        {
            GameObject go = Instantiate(task, new Vector3(previousPos.x, previousPos.y, 0), Quaternion.identity, gameObject.transform);
            previousPos.y -= 60;
            go.transform.localScale = transform.localScale;
        }

        gameObject.GetComponentInChildren<Text>().text = "Tasks (" + completedTasks + "/" + quest.tasks.Count + ")";
    }

    public void CheckQuest(string tag, Actions action)
    {
        QuestTask qT;

        foreach (Transform task in transform)
        {
            qT = task.GetComponent<QuestTask>();

            if(task.tag == tag && qT.actionNeeded == action && !qT.condition)
            {
                completedTasks++;
                qT.condition = true;
                task.GetComponent<Text>().fontStyle = FontStyle.Italic;
                gameObject.GetComponentInChildren<Text>().text = "Tasks (" + completedTasks + "/" + quest.tasks.Count + ")";
                break;
            }
        }
    }
}
