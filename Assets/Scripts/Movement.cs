using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    private Rigidbody2D m_Rigidbody2D;
    private bool facingRight = true;  // For determining which way the player is currently facing.

    public AudioSource source;
    public AudioClip andar;
    public AudioClip correr;

    Vector2 movement;

    public float walkSpeed = 5f;
    public float runSpeed = 7f;

    private bool running = false;
    private bool firstTime = true;

    public Button shiftButton;
    public Button wButton;
    public Button aButton;
    public Button sButton;
    public Button dButton;

    public Sprite bigButton;
    public Sprite bigButtonClicked;
    public Sprite smallButton;
    public Sprite smallButtonClicked;

    private Gauge gauge;
    private string area;
    private float time;
    public float walkingCloseIncrease;
    public float runningIncrease;
    public float runningCloseIncrease;
    public float restDecrease;

    public Animator animator;
    public AudioClip openingDoor;
    public AudioClip closingDoor;

    public SceneController sceneController;
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        gauge = GameObject.Find("Gauge").GetComponent<Gauge>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        running = Input.GetKey(KeyCode.LeftShift);

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("lastHorizontal", movement.x);
            animator.SetFloat("lastVertical", movement.y);
        }

        if (Input.GetKey(KeyCode.LeftShift))
            shiftButton.image.sprite = bigButtonClicked;
        else
            shiftButton.image.sprite = bigButton;

        if (Input.GetKey(KeyCode.W))
            wButton.image.sprite = smallButtonClicked;
        else
            wButton.image.sprite = smallButton;

        if (Input.GetKey(KeyCode.A))
            aButton.image.sprite = smallButtonClicked;
        else
            aButton.image.sprite = smallButton;

        if (Input.GetKey(KeyCode.S))
            sButton.image.sprite = smallButtonClicked;
        else
            sButton.image.sprite = smallButton;

        if (Input.GetKey(KeyCode.D))
            dButton.image.sprite = smallButtonClicked;
        else
            dButton.image.sprite = smallButton;
        if(Input.GetKey(KeyCode.Escape))
            GameObject.Find("SceneController").GetComponent<SceneController>().ChangeScene("Menu");

        time += Time.deltaTime;

        //Fill gauge
        if (time > 1f)
        {
            switch (area)
            {
                case "WalkingArea":
                    if (movement.x != 0 || movement.y != 0)
                    {
                        if (running)
                        {
                            //Running on walking area
                            gauge.setGauge(runningCloseIncrease);
                        }
                        else
                        {
                            //Walking on walking area
                            gauge.setGauge(walkingCloseIncrease);
                        }
                        time = 0;
                    }
                    break;
                case "RunningArea":
                    if (movement.x != 0 || movement.y != 0)
                    {
                        if (running)
                        {
                            //Running on RunningArea
                            gauge.setGauge(runningIncrease);
                            time = 0;
                        }
                    }
                    break;
                default:
                    gauge.setGauge(-restDecrease);
                    time = 0;
                    break;
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (!running)
            Move(walkSpeed);
        else
            Move(runSpeed);
    }

    public void Move(float speed)
    {
        // Move character
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement * speed * Time.fixedDeltaTime);

        // Set animation parameters
        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0)
        {
            bool change = false;
            if (speed == walkSpeed && source.clip != andar)
            {
                source.clip = andar;
                change = true;
            }
            else if (speed == runSpeed && source.clip != correr)
            {
                source.clip = correr;
                change = true;
            }
            source.loop = true;

            

            if (change)
            {
                source.Stop();
                source.Play();
            }
            if (!source.isPlaying)
                source.Play();
        }
        else
            source.Stop();
        // If the input is moving the player right and the player is facing left...
        if (movement.x < 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (movement.x > 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            area = collision.tag;
        }
    
        if(collision.gameObject.layer == 20 && firstTime)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.GetComponent<AudioSource>().Stop();
            collision.GetComponent<AudioSource>().clip = openingDoor;
            collision.GetComponent<AudioSource>().Play();

            firstTime = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (collision.tag == "WalkingArea")
                area = "RunningArea";
            else
                area = "";
        }

        if (collision.gameObject.layer == 20)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            collision.GetComponent<AudioSource>().Stop();
            collision.GetComponent<AudioSource>().clip = closingDoor;
            collision.GetComponent<AudioSource>().Play();

            firstTime = true;
        }
    }
}
