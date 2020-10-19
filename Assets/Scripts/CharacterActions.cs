using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActions : MonoBehaviour
{

    private Collider2D collisionItem;
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
                    item = collisionItem;
                    img.sprite = collisionItem.GetComponent<SpriteRenderer>().sprite;
                    img.SetNativeSize();
                    img.transform.localScale = collisionItem.transform.localScale;
                    img.enabled = true;
                    collisionItem.transform.parent = gameObject.transform;
                    collisionItem.transform.position = gameObject.transform.position;
                    collisionItem.GetComponent<SpriteRenderer>().enabled = false;
                    picked = true;
                    if (filled)
                        fillUp = true;
                    QuestItem(Actions.PickUp);
                }
            }
            else
            {
                if (collisionItem != null)
                {
                    item = null;
                    collisionItem.GetComponent<SpriteRenderer>().enabled = true;
                    collisionItem.transform.parent = null;
                    collisionItem.transform.position = new Vector3(collisionItem.transform.position.x, collisionItem.transform.position.y, 1);
                    collisionItem = null;
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
            img.sprite = collisionItem.GetComponent<SpriteRenderer>().sprite;
            img.SetNativeSize();
            img.transform.localScale = collisionItem.transform.localScale;
            img.enabled = true;
            QuestItem(Actions.FillUp);
        }
    }

    public Collider2D getPicked()
    {
        return collisionItem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collide = true;
            collisionItem = collision;
        }

        if(collision.gameObject.tag == "Sink" && collisionItem != null)
        {
            if(collisionItem.tag == "Bucket" && picked)
            {
                collisionItem.GetComponent<Animator>().SetBool("filled", true);
                fillUp = true;
            }
        }

        if(collision.gameObject.tag == "Hit" && item != null)
        {
            if(item.tag == "Bucket" && filled)
            {
                QuestItem(Actions.Splash);
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
        questTab.GetComponent<QuestTab>().CheckQuest(collisionItem.gameObject.tag, action);
    }
}
