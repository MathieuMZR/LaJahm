using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
public class InfiniteMovement : MonoBehaviour
{
    public SpriteRenderer sprite;
    
    // je t'aime
    [SerializeField] [Header("Movement")] public float moveSpeed;
    [SerializeField] private float slideSpeed = 5f;
    public bool isSliding = false;

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
    [SerializeField] private BoxCollider2D slideColl;
    [SerializeField] public BoxCollider2D regularColl;

    [SerializeField] [Header("Animations")]
    public Animator anim;
    
    
    private void Awake() // récupère les components
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        //rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
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
        
        // switch (jumpBufferCounter)
        // {
        //     case 1:
        //         wallOnLeft = true;
        //         break;
        //     case 2:
        //         wallOnLeft = false;
        //         break;
        //     default:
        //         wallOnLeft = true;
        //         break;
        // }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            prefromSlide();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
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
        if (Physics2D.OverlapBox(col.bounds.center - Vector3.up * col.bounds.size.y / 2, new Vector2(col.bounds.size.x/1.1f, groundedBoxSize), 0f, groundLayer)) isGrounded = true;
        else
        {
            if (!isGrounded) return;
            if (!resetSetGroundedFalseDelay) return;
            if (co != null) StopCoroutine(co);
            co = SetGroundedFalseDelay();
            StartCoroutine(co);
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

    private void prefromSlide()
    {
        isSliding = true;
        
        anim.SetBool ("IsSlide", true);

        groundedBoxSize = 0f;
        slideColl.enabled = true;

        rb.AddForce(Vector2.right * slideSpeed);

        StartCoroutine("stopSlide");
    } 

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.8f);
        anim.Play("Idle");
        anim.SetBool("IsSlide", false);
        regularColl.enabled = true;
        slideColl.enabled = false;
        isSliding = false;
    }
}

