using System;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [Header ("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool canMove = true;
    
    [Header ("Jumping")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("GroundCheck")] 
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    //private bool isGrounded = true;
    
    private Vector2 feetPos;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DetectObstacle();
    }

    private void FixedUpdate()
    {
        if (canMove)
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
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;
        
        //Debug.Log("sending raycast");
        
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance);
        
        //Debug.DrawRay(origin, direction * raycastDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            if (isGrounded())
            {
                Debug.Log("i wanna jump sooo bad");
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
                    Debug.Log("i wanna jump sooo bad");
                    Jump();
                }
            }
            
            if (hit.collider != null) 
                Debug.Log("" + hit.collider.tag);
        }
    }

    
    private bool isGrounded()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(0.25f, -0.4f);;
        
        Debug.Log("NOT ON THE GROUND AAAAAAA");
        
        return Physics2D.OverlapCircle(feetPos, groundCheckRadius, groundLayer);
    }

    private bool isGroundContinuing()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(0.5f, -0.45f);;
        
        Debug.Log("ITS A LEAP OF FAITH");
        
        return Physics2D.OverlapCircle(feetPos, groundCheckRadius, groundLayer);
    }
    
    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        feetPos = origin + new Vector2(0.25f, -0.4f);
        Vector2 direction = Vector2.right;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(feetPos, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, direction * raycastDistance);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
