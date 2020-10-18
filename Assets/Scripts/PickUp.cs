using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{

    private Collider2D item;
    private bool collide = false;
    private bool picked = false;
    private bool fillUp = false;
    private bool filled = false;

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
            if (!picked)
            {
                if (collide)
                {
                    img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                    img.SetNativeSize();
                    img.transform.localScale = item.transform.localScale;
                    img.enabled = true;
                    item.transform.parent = gameObject.transform;
                    item.transform.position = gameObject.transform.position;
                    item.GetComponent<SpriteRenderer>().enabled = false;
                    picked = true;
                    if (filled)
                        fillUp = true;
                    QuestItem(Actions.PickUp);
                }
            }
            else
            {
                if (item != null)
                {
                    item.GetComponent<SpriteRenderer>().enabled = true;
                    item.transform.parent = null;
                    item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1);
                    item = null;
                    img.sprite = null;
                    img.enabled = false;
                    picked = false;
                    fillUp = false;
                }
            }

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceButton.image.color = Color.white;
        }

        if (fillUp)
        {
            filled = true;
            img.sprite = item.GetComponent<SpriteRenderer>().sprite;
            img.SetNativeSize();
            img.transform.localScale = item.transform.localScale;
            img.enabled = true;
            QuestItem(Actions.FillUp);
        }
    }

    public Collider2D getPicked()
    {
        return item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collide = true;
            item = collision;
        }

        if(collision.gameObject.tag == "Sink" && item != null)
        {
            if(item.tag == "Bucket" && picked)
            {
                item.GetComponent<Animator>().SetBool("filled", true);
                fillUp = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collide = false;
        }
    }

    private void QuestItem(Actions action)
    {
        questTab.GetComponent<QuestTab>().CheckQuest(item.gameObject.tag, action);
    }
}
