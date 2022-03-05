using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
public class InfiniteMovement : MonoBehaviour
{
    // je t'aime
    [SerializeField] [Header("Movement")] public float moveSpeed;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] [Range(0f, 100f)] float airSpped;
    [SerializeField] [Range(0f, 2f)] float power;
    
    [Header("Jump")] 
    [SerializeField] bool isGrounded;
    
    [SerializeField] int extraJumpsValue;
    [SerializeField] int extraJumps;

    [SerializeField] [Range(0f, 15f)] float jumpForce;
    [SerializeField] [Range(0f, 1f)] float airBuff;
    [SerializeField] [Range(0f, 3f)] float fallGravityMultiplier;
    
    [SerializeField] [Range(0f, 1f)] float coyoteTime; 
    float coyoteTimeCounter;
    
    [SerializeField] [Range(0f, 1f)] float jumpBufferTime; 
    float jumpBufferCounter;
    
    float gravityScale = 2;
    
    [Header("Collisions")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundedBoxSize;
    [SerializeField] private float wallBoxSize;
    [SerializeField] private bool wallOnLeft;
    [SerializeField] private bool wallOnRight;
    
    private void Awake() // récupère les components
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
        //rb.velocity = new Vector2(rb.velocity.x * moveSpeed/2.1f, rb.velocity.y);
        //rb.velocity = (new Vector2(moveSpeed, rb.velocity.y));
        //if (rb.velocity.x > 30)
        //{
            //rb.velocity = new Vector2(30, rb.velocity.y);
        //}
        
        GroundCheck();
        ManageCoyoteTime();
        ManageInputs();
        ManageGravity();

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
    }
    
    void ManageInputs()
    {
        // Gère le jumpBuffer
        if (Input.GetButtonDown("Jump")) 
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Saut
        if (jumpBufferCounter > 0f)
        {
            Jump();
        } 

        // Maintien en l'air du saut
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * airBuff);
            coyoteTimeCounter = 0f;
        }
        
    }

    void ManageGravity()
    {
        if (rb.velocity.y < 0) rb.gravityScale = gravityScale * fallGravityMultiplier; 
        else rb.gravityScale = gravityScale;
    }
        
    #region Ground & Wall Checks
    
    private IEnumerator co;
    private bool resetSetGroundedFalseDelay = true;
    
    void GroundCheck() // Fait une box en dessous du joueur pour dectecter si le joueur touche le sol
    {
        if (Physics2D.OverlapBox(col.bounds.center - Vector3.up * col.bounds.size.y / 2, new Vector2(col.bounds.size.x/1.1f, groundedBoxSize), 0f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                if (resetSetGroundedFalseDelay)
                {
                    if (co != null)
                    {
                        StopCoroutine(co);
                    }
                    co = SetGroundedFalseDelay();
                    StartCoroutine(co);
                }
            }
        }
    }
    
    IEnumerator SetGroundedFalseDelay()
    {
        resetSetGroundedFalseDelay = false;
        yield return new WaitForSeconds(coyoteTime);
        isGrounded = false;
        resetSetGroundedFalseDelay = true;
    }
    
    void ManageCoyoteTime()
    {
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }
    
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }
        else if (extraJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumps--;
            jumpBufferCounter = 0f; 
        }
    }
    
    #endregion
}
