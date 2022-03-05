using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))] // vérifie que le gameobject a bien les components 
public class PlayerController : MonoBehaviour
{
    [SerializeField] bool facingRight;
    [Header("Components")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer spriteRenderer;

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
    
    float gravityScale = 1;
    
    [Header("Gliding")] 
    [SerializeField] bool isGliding;

    [SerializeField] [Range(1f, 3f)] float gravityReductionWhileGliding;
    [SerializeField] [Range(1f, 1.05f)]  float moveXMultiplierWhileGliding;
    
    [Header("WallSlide")]
    [SerializeField] bool isWallSliding;
    
    [SerializeField] [Range(0f, 3f)] float wallSlideSpeed;

    [Header("WallJump")]
    [SerializeField]  [Range(0f, 15f)] float wallJumpForce;
    [SerializeField] [Range(0f, 1f)] float wallJumpCd;
    
    [SerializeField] [Range(0f, 1f)] float wallJumpBufferTime;
    private float wallJumpBufferCounter;

    [Header("Dash")]
    [SerializeField] bool haveDash;
    [SerializeField] bool canDash;
    [SerializeField] bool isDashing;
    
    [SerializeField]  [Range(0f, 2f)] float dashDuration;
    [SerializeField]  [Range(0f, 50f)] float dashForce;

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
        GroundCheck();
        WallCheck();
        DashCheck();
        ManageCoyoteTime();
        ManageInputs();
        ManageGravity();

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            haveDash = true;
        }
    }

    void ManageInputs()
    {
        // Gère le jumpBuffer
        if (Input.GetButtonDown("Jump")) 
        {
            jumpBufferCounter = jumpBufferTime;
            wallJumpBufferCounter = wallJumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            wallJumpBufferCounter -= Time.deltaTime;
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
        
        // Glide
        if (Input.GetButton("Jump") && rb.velocity.y < 0f)
        {
            isGliding = true;
            rb.velocity = new Vector2(rb.velocity.x * moveXMultiplierWhileGliding, rb.velocity.y / gravityReductionWhileGliding);
        }
        else isGliding = false;
        
        // Dash
        if (Input.GetButtonDown("Dash") && canDash)
        {
           StartCoroutine(Dash());
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
    
    void WallCheck() // Fait des box sur les coté du joueur pour dectecter si le joueur touche un mur
    {
        if (Physics2D.OverlapBox(col.bounds.center + Vector3.right * col.bounds.size.x / 2, new Vector2(wallBoxSize, col.bounds.size.y / 1.5f), 0f, groundLayer))
        {
            wallOnRight = true; 
        } 
        else wallOnRight = false;

        if (Physics2D.OverlapBox(col.bounds.center - Vector3.right * col.bounds.size.x / 2, new Vector2(wallBoxSize, col.bounds.size.y / 1.1f), 0f, groundLayer))
        {
            wallOnLeft = true;
        } 
        else wallOnLeft = false;
    }
    #endregion

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
    
    #region  Dash
    void DashCheck()
    {
        if (!isGrounded && !isDashing && haveDash) canDash = true;
        else canDash = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        haveDash = false;
        
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        if (facingRight)
        {
            rb.AddForce(new Vector2(dashForce * 2, 0), ForceMode2D.Impulse);
        }
        if (!facingRight)
        {
            rb.AddForce(new Vector2(-dashForce * 2,0 ), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = gravityScale;
    }
    #endregion
   
    private void OnDrawGizmos() // Dessine les boxs dans la scene pour les Physcis Check
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center - Vector3.up * col.bounds.size.y / 2, new Vector2(col.bounds.size.x /1.5f, groundedBoxSize)); 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(col.bounds.center + Vector3.right * col.bounds.size.x / 2, new Vector2(wallBoxSize, col.bounds.size.y/1.1f)); 
        Gizmos.DrawWireCube(col.bounds.center - Vector3.right * col.bounds.size.x / 2, new Vector2(wallBoxSize, col.bounds.size.y/1.1f));
    }
}