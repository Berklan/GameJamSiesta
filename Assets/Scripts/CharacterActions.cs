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
    private bool litUp = false;
    private bool lit = false;
    private bool dropable = false;
    private bool dropped = false;
    private bool creamed = false;
    private bool onAlarm = false;
    private bool canSetAlarm = false;
    private bool alarmIsSet = false;
    private bool canPickAlarm = false;

    public Button spaceButton;
    public GameObject selectedItem;

    private GameObject questTab;

    public GameObject timer;

    public GameObject balloonVuvuzela;
    public GameObject fireworksBucket;

    private void Awake()
    {
        selectedItem.GetComponent<Image>().enabled = false;
        GameObject.Find("Cream").GetComponent<SpriteRenderer>().enabled = false;
        questTab = GameObject.Find("QuestTab");
    }

    private void Update()
    {
        Image img = selectedItem.GetComponent<Image>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onAlarm && !canPickAlarm)
            {
                Timer alarm = timer.GetComponent<Timer>();
                alarm.SetNewTimer(alarm.GetTimer() - 5f);
            }

            // Pick up item
            if (!picked)
            {
                if (collide || (canPickAlarm && onAlarm))
                {
                    if (canPickAlarm && onAlarm)
                    {
                        item = GameObject.Find("Alarm").transform.GetChild(0).gameObject;
                        item.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        canPickAlarm = false;
                        alarmIsSet = false;
                    }
                    else
                    {
                        item = collisionItem;                   
                    }
                    item.transform.parent = gameObject.transform;
                    item.transform.position = gameObject.transform.position;
                    img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                    img.SetNativeSize();
                    img.transform.localScale = item.transform.localScale * 2;
                    img.enabled = true;   
                    item.GetComponent<SpriteRenderer>().enabled = false;
                    picked = true;
                    if (filled)
                        fillUp = true;
                    if (lit)
                        litUp = true;
                    if (item.CompareTag("FireworksBucket") && dropped)
                        dropped = false;

                    QuestItem(item.tag, Actions.PickUp);
                }
            }
            else
            {
                // Fuse items
                if (collide)
                {
                    if ((item.CompareTag("Vuvuzela") && collisionItem.CompareTag("Balloon")) || (item.CompareTag("Balloon") && collisionItem.CompareTag("Vuvuzela")))
                    {
                        FuseObjects(balloonVuvuzela, Actions.PickUp);
                        if (item.CompareTag("Vuvuzela"))
                            QuestItem(item.tag, Actions.PickUp);
                        else
                            QuestItem(collisionItem.tag, Actions.PickUp);
                    }
                    else if ((item.CompareTag("Bucket") && collisionItem.CompareTag("Fireworks")) || (item.CompareTag("Fireworks") && collisionItem.CompareTag("Bucket")))
                    {
                        if (!fillUp)
                        {
                            FuseObjects(fireworksBucket, Actions.PickUp);
                            if(item.CompareTag("Bucket"))
                                QuestItem(item.tag, Actions.PickUp);
                            else
                                QuestItem(collisionItem.tag, Actions.PickUp);

                            Debug.Log("Fireworks in bucket");
                        }
                        else
                        {
                            Debug.Log("Game Over - Fireworks ruined");
                            item.GetComponent<Animator>().SetBool("lit", false);
                        }
                    }
                    else
                    {
                        // Swap items
                        if (collisionItem != item)
                        {
                            GameObject currentItem = item.gameObject;

                            item = collisionItem;
                            img.sprite = item.GetComponent<SpriteRenderer>().sprite;
                            img.SetNativeSize();
                            img.transform.localScale = item.transform.localScale * 2;
                            img.enabled = true;
                            item.transform.parent = gameObject.transform;
                            item.transform.position = gameObject.transform.position;
                            item.GetComponent<SpriteRenderer>().enabled = false;

                            currentItem.GetComponent<SpriteRenderer>().enabled = true;
                            currentItem.transform.parent = null;
                            currentItem.transform.position = new Vector3(currentItem.transform.position.x, currentItem.transform.position.y, 1);

                            QuestItem(item.tag, Actions.PickUp);
                        }
                    }
                }
                else
                // Leave item on the floor
                {
                    if (dropable)
                    {
                        dropped = true;
                        QuestItem(item.tag, Actions.Drop);
                        dropable = false;
                    }
                    item.GetComponent<SpriteRenderer>().enabled = true;

                    if (canSetAlarm && onAlarm)
                    {
                        canSetAlarm = false;
                        alarmIsSet = true;
                        canPickAlarm = true;
                        GameObject alarm = GameObject.Find("Alarm");

                        item.transform.parent = alarm.transform;
                        item.transform.position = new Vector3(alarm.transform.position.x, alarm.transform.position.y, 0);
                        item.GetComponent<SpriteRenderer>().sortingLayerName = "Item";
                        QuestItem(item.tag, Actions.Drop);
                    }
                    else
                    {
                        item.transform.parent = null;
                        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1);
                    }
                    
                    item = null;
                    img.sprite = null;
                    img.enabled = false;
                    picked = false;
                    fillUp = false;
                    litUp = false;
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
            img.sprite = item.GetComponent<SpriteRenderer>().sprite;
            img.SetNativeSize();
            img.transform.localScale = item.transform.localScale * 2;
            img.enabled = true;
            QuestItem(item.tag, Actions.FillUp);
        }

        if (litUp)
        {
            lit = true;
            img.sprite = item.GetComponent<SpriteRenderer>().sprite;
            img.SetNativeSize();
            img.transform.localScale = item.transform.localScale * 2;
            img.enabled = true;
            QuestItem(item.tag, Actions.LightUp);
        }

        if(timer.GetComponent<Timer>().GetTimer() <= 0f)
        {
            if (dropped || canSetAlarm)
            {
                // WIN
                Debug.Log("WIN");
            }
            else
            {
                // LOSE
                Debug.Log("LOSE");
            }

        }
    }

    private void FuseObjects(GameObject newObject, Actions action) 
    {
        GameObject oldItem = item;
        Image img = selectedItem.GetComponent<Image>();

        item = Instantiate(newObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity, gameObject.transform);
        img.sprite = item.GetComponent<SpriteRenderer>().sprite;
        img.SetNativeSize();
        img.transform.localScale = item.transform.localScale * 2;
        img.enabled = true;
        item.transform.parent = gameObject.transform;
        item.transform.position = gameObject.transform.position;
        item.GetComponent<SpriteRenderer>().enabled = false;

        Destroy(oldItem);
        Destroy(collisionItem);
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

        if (collision.gameObject.CompareTag("Sink") && item != null)
        {
            if(item.CompareTag("Bucket") && picked && !litUp && !fillUp)
            {
                item.GetComponent<Animator>().SetBool("filled", true);
                fillUp = true;
            }
        }

        if (collision.gameObject.CompareTag("Stove") && item != null)
        {
            if (item.CompareTag("FireworksBucket") && picked && !fillUp && !litUp)
            {
                // It's lit, set timer for gameover
                item.GetComponent<Animator>().SetBool("lit", true);
                timer.GetComponent<Timer>().SetNewTimer(10f);
                QuestItem(item.tag, Actions.LightUp);
                litUp = true;
            }
        }

        if (collision.CompareTag("Alarm"))
        {
            onAlarm = true;

            if (item == null)
            {
                if (alarmIsSet)
                    canPickAlarm = true;
            }
            else
            {
                if (item.CompareTag("Trap"))
                {
                    canSetAlarm = true;
                }
            }
        }

        // Contact with dad
        if (collision.gameObject.CompareTag("Hit") && item != null)
        {
            if(item.gameObject.CompareTag("Bucket") && filled)
            {
                QuestItem(item.tag, Actions.Splash);
            }

            if (item.gameObject.CompareTag("BalloonVuvuzela"))
            {
                QuestItem(item.tag, Actions.Blow);
            }

            if (item.gameObject.CompareTag("FireworksBucket") && lit)
            {
                dropable = true;
            }

            if (item.gameObject.CompareTag("ShavingCream"))
            {
                creamed = true;
                GameObject.Find("Cream").GetComponent<SpriteRenderer>().enabled = true;
                QuestItem(item.tag, Actions.FillUp);
            }

            if (item.gameObject.CompareTag("Feather"))
            {
                QuestItem(item.tag, Actions.PickNose);
                if (creamed)
                {
                    // WIN
                    Debug.Log("WIN");
                }
                else
                {
                    // LOSE didnt complete all the tasks
                    Debug.Log("LOSE");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collide = false;
        }

        if (collision.gameObject.CompareTag("Hit") && item != null)
        {
            dropable = false;
        }

        if (collision.CompareTag("Alarm"))
        {
            onAlarm = false;
        }
    }

    private void QuestItem(string tag, Actions action)
    {
        questTab.GetComponent<QuestTab>().CheckQuest(tag, action);
    }

    private void GameOver(string tag)
    {
        //GameObject.Find("SceneController").GetComponent<SceneController>().GameOverMessage(tag);
    }
}
