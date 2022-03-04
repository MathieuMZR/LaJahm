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

    [Header("Vitesse")] 
    [SerializeField] bool canRun;

    [SerializeField] [Range(0f, 10f)] float runSpeed;
    [SerializeField] [Range(0f, 10f)] float airSpped;
    [SerializeField] [Range(0f, 2f)] float power;
    [SerializeField] [Range(0f, 20f)] float acceleration;
    [SerializeField] [Range(0f, 20f)] float deceleration;
    
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

    void Start()
    {
        canRun = true;
    }

    void Update()
    {
        Flip(rb.velocity.x); // Pour flip le sprite du joueur dans la bonne direction
        GroundCheck();
        WallCheck();
        DashCheck();
        ManageCoyoteTime();
        ManageWallSlide();
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
        if (canRun) Run(Input.GetAxisRaw("Horizontal"));
        
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

        // lance le WallJump
        if (isWallSliding && !isGrounded && Input.GetButton("Jump") && !isGliding)
        {
            WallJump();
            wallJumpBufferCounter = 0f;
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

    #region Run
    void Run(float moveInput)
    {
        if (coyoteTimeCounter > 0f)
        {
            float targetSpeed = moveInput * runSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelerate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration: deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelerate, power) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);
        }
        else
        {
            float targetSpeed = moveInput * airSpped;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelerate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration: deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelerate, power) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);
        }
    }
    #endregion
    
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

    #region WallJump
    void ManageWallSlide()
    {
        if ((wallOnLeft || wallOnRight) && !isGrounded && rb.velocity.y<0 ) isWallSliding = true;
        else isWallSliding = false;
        
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    void WallJump()
    {
        canRun = false;
        extraJumps++;
        
        if (wallOnLeft) rb.AddForce(new Vector2(Vector2.right.x * wallJumpForce, Vector2.up.y * wallJumpForce), ForceMode2D.Impulse);

        if (wallOnRight) rb.AddForce(new Vector2(-Vector2.right.x * wallJumpForce, Vector2.up.y * wallJumpForce), ForceMode2D.Impulse);
        
        StartCoroutine(WallJumpCd());
    }
    IEnumerator WallJumpCd()
    {
        yield return new WaitForSeconds(wallJumpCd);
        canRun = true;
    }
    #endregion

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

    void Flip(float _velocity) //Flip le sprite du joueur pour qu'il regarde dans la bonne direction
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false; 
            facingRight = true;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true; 
            facingRight = false;
        }
    }
}