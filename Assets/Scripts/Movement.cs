using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    public float walkSpeed = 5f;
    public float runSpeed = 7f;

    private bool running = false;

    public Button shiftButton;
    public Button wButton;
    public Button aButton;
    public Button sButton;
    public Button dButton;

    private Gauge gauge;
    private string area;
    private float time;
    public float walkingCloseIncrease;
    public float runningIncrease;
    public float runningCloseIncrease;
    public float restDecrease;

    public Animator animator;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        gauge = GameObject.Find("Gauge").GetComponent<Gauge>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        running = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKey(KeyCode.LeftShift))
            shiftButton.image.color = Color.red;
        else
            shiftButton.image.color = Color.white;

        if (Input.GetKey(KeyCode.W))
            wButton.image.color = Color.red;
        else
            wButton.image.color = Color.white;

        if (Input.GetKey(KeyCode.A))
            aButton.image.color = Color.red;
        else
            aButton.image.color = Color.white;

        if (Input.GetKey(KeyCode.S))
            sButton.image.color = Color.red;
        else
            sButton.image.color = Color.white;

        if (Input.GetKey(KeyCode.D))
            dButton.image.color = Color.red;
        else
            dButton.image.color = Color.white;

        time += Time.deltaTime;

        //Fill gauge
        if (time > 1f)
        {
            switch (area)
            {
                case "WalkingArea":
                    if (horizontalMove != 0 || verticalMove != 0)
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
                    if (horizontalMove != 0 || verticalMove != 0)
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
            Move(horizontalMove, verticalMove, walkSpeed);
        else
            Move(horizontalMove, verticalMove, runSpeed);
    }

    public void Move(float horizontal, float vertical, float speed)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizontal * speed, vertical * speed);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // Set animation parameters
        animator.SetFloat("horizontalSpeed", Mathf.Abs(horizontal * speed));
        animator.SetFloat("verticalSpeed", vertical * speed);

        // If the input is moving the player right and the player is facing left...
        if (horizontal < 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontal > 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

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
    
        if(collision.gameObject.layer == 20)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
        }
    }
}
