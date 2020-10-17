using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{

    private Collider2D item;
    private bool collide = false;

    public Button spaceButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceButton.image.color = Color.red;
            if (collide)
            {
                item.transform.parent = gameObject.transform;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceButton.image.color = Color.white;
            if(item != null)
            {
                item.transform.parent = null;
                item = null;
            }
        }
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
}
