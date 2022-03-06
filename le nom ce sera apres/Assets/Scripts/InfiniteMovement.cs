using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public float slideMultiplier = 1.2f;
    public int scorePlayer;

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

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    public static InfiniteMovement instance;
    private void Awake() // récupère les components
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (instance == null) instance = this;
    }
    
    void Update()
    {
        ManageCoyoteTime();
        ManageInputs();
        ManageGravity();

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            prefromSlide();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
        anim.SetFloat("Run", 1);
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
        
        //slide
        if (Input.GetButtonDown("Slide") && rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * slideMultiplier, rb.velocity.y);
            anim.SetBool("Slide", true);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
        anim.SetBool("jump",false);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(col.bounds.center - Vector3.up * col.bounds.size.y / 2, 
            new Vector3(col.bounds.size.x/1.1f, groundedBoxSize,0));
    }
    
    IEnumerator SetGroundedFalseDelay()
    {
        isGrounded = false;
        //resetSetGroundedFalseDelay = false;
        yield return new WaitForSeconds(coyoteTime);
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
            anim.SetBool("jump", true);
            audioSource2.Play();
            isGrounded = false;
        }
        else if (extraJumps > 0 && !isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumps--;
            jumpBufferCounter = 0f;
            anim.Play("Jump", -1, 0);
            audioSource2.Play();
        }
    }
    
    #endregion

    private void prefromSlide()
    {
        isSliding = true;
        
        //anim.SetBool ("IsSlide", true);

        //groundedBoxSize = 0f;
        //slideColl.enabled = true;

        //rb.AddForce(Vector2.right * slideSpeed);
        slideColl.size = new Vector2(1f, 1.05f);
        audioSource1.Play();
        StartCoroutine(stopSlide());
        anim.SetBool("Slide", true);
        
    } 

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(0.8f);
        //anim.Play("Idle");
        //anim.SetBool
        //("IsSlide", false);
        slideColl.size = new Vector2(1f, 2f);
        //regularColl.enabled = true;
        //slideColl.enabled = true;
        isSliding = false;
        yield return null;
        anim.SetBool("Slide", false);
    }

  /*  void PlayFootSteps()
    {
        audioSource3.Play();
        StartCoroutine(waitFootSteps());

    }

    IEnumerator waitFootSteps()
    {
        yield return new WaitForSeconds(0.3f);
    }
    */

}

