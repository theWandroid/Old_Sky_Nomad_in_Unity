using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 2;
    public float jumpSpeed = 3.5f;
    public float DobleJumpSpeed = 3;

    private bool canDobleJump;


    Rigidbody2D rb2D;

    public bool betterJump = false;
    public float fallMultiplier = 0.5f;
    public float lowJumpMultiplier = 1f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private const string AXIS_H = "Horizontal";


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKey("space")  || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (CheckGround.isGrounded)
            {
                canDobleJump = true;
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
            }
            else
            {
                if (Input.GetKeyDown("space") || CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    if (canDobleJump)
                    {
                        animator.SetBool("DobleJump", true);
                        rb2D.velocity = new Vector2(rb2D.velocity.x, DobleJumpSpeed);
                        canDobleJump = false;
                    }
                }
            }
            
        }
    }


    void FixedUpdate()
    {

#if UNITY_STANDALONE_WIN

        if (Input.GetKey("d") || Input.GetKey("right")) 
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool("Run", true);
        }

        else if (Input.GetKey("a") || Input.GetKey("left"))
            
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool("Run", true);
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            animator.SetBool("Run", false);
        }

#endif

        if (CrossPlatformInputManager.GetAxis(AXIS_H) != 0)
        {
            if (CrossPlatformInputManager.GetAxis(AXIS_H) > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            rb2D.velocity = new Vector2(CrossPlatformInputManager.GetAxis(AXIS_H), rb2D.velocity.y).normalized * runSpeed;
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }


        if (betterJump)
        {
            if (rb2D.velocity.y<0)
            {
                rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
            }
            if (rb2D.velocity.y>0 && !Input.GetKey("space") || rb2D.velocity.y > 0 && !CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        if (CheckGround.isGrounded == false)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
        }

        if (CheckGround.isGrounded == true)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("DobleJump", false);
            animator.SetBool("Falling", false);
        }
        if (rb2D.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
        }
        else if (rb2D.velocity.y > 0)
        {
            animator.SetBool("Falling", false);
        }

        if (CrossPlatformInputManager.GetAxis(AXIS_H) != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
}
