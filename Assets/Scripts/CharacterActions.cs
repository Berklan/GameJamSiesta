using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActions : MonoBehaviour
{

    private GameObject collisionItem;
    private GameObject item;
    private bool collide = false;
    private bool picked = false;
    private bool fillUp = false;
    private bool filled = false;

    public Button spaceButton;
    public GameObject selectedItem;

    private GameObject questTab;

    public GameObject balloonVuvuzela;

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
            spaceButton.image.sprite = gameObject.GetComponent<Movement>().bigButtonClicked;

            // Pick up item
            if (!picked)
            {
                if (collide)
                {
                    item = collisionItem;
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
                if (collide)
                {
                    if ((item.CompareTag("Vuvuzela") && collisionItem.CompareTag("Balloon")) || (item.CompareTag("Balloon") && collisionItem.CompareTag("Vuvuzela")))
                    {
                        GameObject oldItem = item;

                        item = Instantiate(balloonVuvuzela, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, gameObject.transform);
                        img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                        img.SetNativeSize();
                        img.transform.localScale = item.transform.localScale;
                        img.enabled = true;
                        item.transform.parent = gameObject.transform;
                        item.transform.position = gameObject.transform.position;
                        item.GetComponent<SpriteRenderer>().enabled = false;

                        Destroy(oldItem);
                        Destroy(collisionItem);
                        QuestItem(Actions.PickUp);
                    }
                    else
                    {
                        if (collisionItem != item)
                        {
                            GameObject currentItem = item.gameObject;

                            item = collisionItem;
                            img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                            img.SetNativeSize();
                            img.transform.localScale = item.transform.localScale;
                            img.enabled = true;
                            item.transform.parent = gameObject.transform;
                            item.transform.position = gameObject.transform.position;
                            item.GetComponent<SpriteRenderer>().enabled = false;

                            currentItem.GetComponent<SpriteRenderer>().enabled = true;
                            currentItem.transform.parent = null;
                            currentItem.transform.position = new Vector3(currentItem.transform.position.x, currentItem.transform.position.y, 1);

                            QuestItem(Actions.PickUp);
                        }
                    }
                }
                else
                // Leave item on the floor
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
            spaceButton.image.sprite = gameObject.GetComponent<Movement>().bigButton;
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

    public GameObject getPicked()
    {
        return collisionItem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Diary"))
        {
            Destroy(collision.gameObject);
            questTab.GetComponent<QuestTab>().OpenQuestTab();
        }

        if (collision.gameObject.layer == 9)
        {
            collide = true;
            collisionItem = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Sink") && collisionItem != null)
        {
            if(collisionItem.CompareTag("Bucket") && picked)
            {
                collisionItem.GetComponent<Animator>().SetBool("filled", true);
                fillUp = true;
            }
        }

        if(collision.gameObject.CompareTag("Hit") && item != null)
        {
            if(item.gameObject.CompareTag("Bucket") && filled)
            {
                QuestItem(Actions.Splash);
            }

            if (item.gameObject.CompareTag("BalloonVuvuzela"))
            {
                QuestItem(Actions.Blow);
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
