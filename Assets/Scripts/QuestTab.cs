using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTab : MonoBehaviour
{

    enum Action
    {
        PickUp,
        FillUp
    }

    public List<GameObject> quests;
    private int completedQuests;
    private Vector3 previousPos;

    private void Awake()
    {
        cleanQuests();
        UpdateQuestList();
    }

    private void cleanQuests()
    {
        foreach(GameObject quest in quests)
        {
            quest.GetComponent<Quests>().condition = false;
            quest.GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    private void UpdateQuestList()
    {
        completedQuests = 0;

        Vector3 parentPos = gameObject.transform.position;

        previousPos = new Vector3(parentPos.x + 15, parentPos.y - 35, 0);

        for (int i = 0; i < quests.Count; i++)
        {
            GameObject go = Instantiate(quests[i], new Vector3(previousPos.x, previousPos.y, 0), Quaternion.identity, gameObject.transform);
            previousPos.y -= 25;
            go.transform.localScale = transform.localScale;
        }

        gameObject.GetComponent<Text>().text = "Tasks (" + completedQuests + "/" + quests.Count + ")";
    }

    public void CheckQuest(string tag, Actions action)
    {
        Quests quest;

        foreach (Transform child in transform)
        {
            quest = child.gameObject.GetComponent<Quests>();

            if(child.tag == tag && quest.actionNeeded == action && !quest.condition)
            {
                completedQuests++;
                quest.condition = true;
                child.gameObject.GetComponent<Text>().fontStyle = FontStyle.Italic;
                gameObject.GetComponent<Text>().text = "Tasks (" + completedQuests + "/" + quests.Count + ")";
                break;
            }
        }
    }
}
