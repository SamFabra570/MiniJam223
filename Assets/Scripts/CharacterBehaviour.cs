using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public static CharacterBehaviour Instance;
    
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    public Transform levelStart;
    
    [Header ("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool isMoving = true;
    
    [Header ("Jumping")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("GroundCheck")] 
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 feetPos;

    [SerializeField] private float xOffset = 0.25f;
    [SerializeField] private float yOffset = -0.7f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DetectObstacle();
        
        if (isBlockStoppingPath()) 
            Debug.Log("IS BLOCK IS BLOCKGIN");

        if (rb.linearVelocityX <= 0.1 && isMoving)
        {
            isMoving = false;
            
            // if (isBlockStoppingPath())
            //     isMoving = false;
        }
        else if (!isMoving)
        {
            if (!isBlockStoppingPath()) 
                isMoving = true;
        }
        
        animator.SetBool("IsGrounded", isGrounded());
        animator.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new  Vector2(0f, rb.linearVelocity.y);
        }
    }

    private void DetectObstacle()
    {
        // if (!isMoving)
        //     return;
        
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;
        
        //Debug.Log("sending raycast");
        
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance);
        
        //Debug.DrawRay(origin, direction * raycastDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            if (isGrounded())
            {
                //Debug.Log("i wanna jump sooo bad");
                Jump();
            }
        }
        else
        {
            //Debug.Log("smth is null buddy");

            if (!isGroundContinuing())
            {
                if (isGrounded())
                {
                    //Debug.Log("i wanna jump sooo bad");
                    Jump();
                }
            }
            
            // if (hit.collider != null) 
            //     Debug.Log("" + hit.collider.tag);
        }
    }

    
    private bool isGrounded()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(xOffset, yOffset);;
        
        //Debug.Log("NOT ON THE GROUND AAAAAAA");
        
        return Physics2D.OverlapCircle(feetPos, groundCheckRadius, groundLayer);
    }

    private bool isGroundContinuing()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(xOffset + 0.65f, yOffset - 0.05f);;
        
        //Debug.Log("ITS A LEAP OF FAITH");
        
        return Physics2D.OverlapCircle(feetPos, groundCheckRadius, groundLayer);
    }
    
    private bool isBlockStoppingPath()
    {
        Vector2 origin = transform.position;
        
        //Debug.Log("ITS A LEAP OF FAITH");

        return Physics2D.OverlapCircle(origin + new Vector2(1, 0.25f), groundCheckRadius, groundLayer);
    }
    
    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(xOffset, yOffset);
        Vector2 direction = Vector2.right;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos, groundCheckRadius);
        
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(origin + new Vector2(1, 0.25f), groundCheckRadius);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(origin, direction * raycastDistance);
    }

    private void Jump()
    {
        if (!isMoving)
            return;
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        
        animator.SetTrigger("Jump");
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("KillBox"))
    //     {
    //         GameManager.Instance.EndGame();
    //     }
    // }

    public void ResetPosition()
    {
        transform.position = levelStart.position;
    }
}
