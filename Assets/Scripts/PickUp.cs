using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{

    private Collider2D item;
    private bool collide = false;

    public Button spaceButton;
    public GameObject selectedItem;

    private GameObject questTab;

    private void Awake()
    {
        selectedItem.GetComponent<Image>().enabled = false;
        questTab = GameObject.Find("QuestTab");
    }

    private void Update()
    {
        Image img = selectedItem.GetComponent<Image>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceButton.image.color = Color.red;
            if (collide)
            {
                img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                img.SetNativeSize();
                img.transform.localScale = item.transform.localScale;
                img.enabled = true;
                item.transform.parent = gameObject.transform;
                item.transform.position = gameObject.transform.position;
                QuestItem();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceButton.image.color = Color.white;
            if(item != null)
            {
                item.transform.parent = null;
                item = null;
                img.sprite = null;
                img.enabled = false;
            }
        }
    }

    public Collider2D getPicked()
    {
        return item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide = true;
        item = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collide = false;
    }

    private void QuestItem()
    {
        questTab.GetComponent<QuestTab>().CheckQuest(item.gameObject.tag, Actions.PickUp);
    }
}
