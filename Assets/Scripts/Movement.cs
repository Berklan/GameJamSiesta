using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private bool m_FactingFront = true;
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
    
    public Image unselectedButton;
    public Image selectedButton;

    private SpriteRenderer characterSprite;
    public Sprite characterFront;
    public Sprite characterBack;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        characterSprite = gameObject.GetComponent<SpriteRenderer>();
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

        // If the input is moving the player right and the player is facing left...
        if (horizontal > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontal < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

        if (vertical > 0 && m_FactingFront)
            FlipVertical();
        else if (vertical < 0 && !m_FactingFront)
            FlipVertical();
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

    private void FlipVertical()
    {
        Collider2D item = gameObject.GetComponent<PickUp>().getPicked();

        m_FactingFront = !m_FactingFront;
        
        if (m_FactingFront)
        {
            characterSprite.sprite = characterFront;
            
            if (item != null)
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1);
        }
        else
        {
            characterSprite.sprite = characterBack;
            if(item != null)
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 0);
        }
    }
}
