using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.01f;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameOverController gameOverController;

    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldDuration;

    private Vector2 originalBoxSize;
    private Vector2 originalBoxOffset;
    private Rigidbody2D rb2D;

    private bool isGrounded;
    private bool wasMoving = false;

    public static bool hasdoubleJump = false;
    private bool canDoubleJump;

    private bool hasShield = false;
    private float shieldTimer = 0f;

    private LifeController lifeController;

    private void Awake()
    {
        Debug.Log("Player Controller awake");
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Store original collider values
        originalBoxSize = boxCollider2D.size;
        originalBoxOffset = boxCollider2D.offset;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool crouchInput = Input.GetKey(KeyCode.LeftControl);

        CheckGrounded();
        HandleMovement(horizontal, jumpInput);
        HandleCrouch(crouchInput);
        UpdateAnimations(horizontal,rb2D.velocity.y);

        if (hasShield)
        {
            shieldTimer -= Time.deltaTime;

            if (shieldTimer < 0f)
            {
                DeactivateShield();
            }
        }
    }
    public void ActiveShield(float duration)
    {
        hasShield = true;
        shieldTimer = duration;

        if(shield != null)
        {
            shield.SetActive(true);
        }
    }

    private void DeactivateShield()
    {
        hasShield = false;
        if(shield != null)
        {
            shield.SetActive(false);
        }
    }

    private void CheckGrounded()
    {
        Vector2 rayStart = boxCollider2D.bounds.center;
        rayStart.y -= boxCollider2D.bounds.extents.y;

        // Shoot ray slightly downward
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        // Visual debug
        Debug.DrawRay(rayStart, Vector2.down * groundCheckDistance,
                     isGrounded ? Color.green : Color.red);
    }
    private void HandleMovement(float horizontal, bool jumpInput)
    {
        // Horizontal movement
        rb2D.velocity = new Vector2(horizontal * speed, rb2D.velocity.y);
        // Handle movement sound
        bool isMoving = Mathf.Abs(horizontal) > 0.1f && isGrounded;

        if (isMoving && !wasMoving)
        {
            // Start playing movement sound when starting to move
            SoundManager.Instance.PlayFootStep(Sounds.PlayerMove);
            SoundManager.Instance.SoundFootStep.GetComponent<AudioSource>().loop = true;
        }
        else if (!isMoving && wasMoving)
        {
            // Stop movement sound when stopping
            SoundManager.Instance.Stop(Sounds.PlayerMove);
            SoundManager.Instance.SoundFootStep.GetComponent<AudioSource>().loop = false;
        }

        wasMoving = isMoving;
        // Jump if grounded and jump input is pressed
        if (jumpInput)
        {
            if(isGrounded) 
            { 
                SoundManager.Instance.Play(Sounds.PlayerJump);
                rb2D.velocity =  new Vector2(rb2D.velocity.x,jumpForce);
                canDoubleJump = true;
            }
            else
            {
                DoubleJump();
            }
        }

    }
    private void DoubleJump()
    {
        if (hasdoubleJump && canDoubleJump )
        {
            SoundManager.Instance.Play(Sounds.PlayerJump);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);

            canDoubleJump = false;
        }
    }
    public void GrantDoubleJump()
    {
        hasdoubleJump = true;
        
        Debug.Log("Dowble Jump Granted!");
    }

    private void HandleCrouch(bool crouchInput)
    {
        if (crouchInput)
        {
            // Crouch collider settings
            boxCollider2D.size = new Vector2(0.6f, 1.28f);
            boxCollider2D.offset = new Vector2(-0.1f, 0.58f);
        }
        else
        {
            // Restore original collider settings
            boxCollider2D.size = originalBoxSize;
            boxCollider2D.offset = originalBoxOffset;
        }

        animator.SetBool("Crouch", crouchInput);
    }

    private void UpdateAnimations(float horizontal,float vertical)
    {
        // Run animation
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetFloat("yVelocity", vertical);
        
        // Flip character sprite based on movement direction
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontal);
            transform.localScale = scale;
        }

        // Jump animation
        animator.SetBool("Jump", !isGrounded);
        Debug.Log("vertical value : " + vertical);
    }

    public void PickUpKey()
    {
        Debug.Log("Player picked up the key");
        scoreController.IncreaseScore(10);
    }

    public void PlayerDeath()
    {
        Debug.Log("PLayer Death by Enemy");
        animator.SetTrigger("Death");
        SoundManager.Instance.Stop(Sounds.PlayerMove);
        StartCoroutine(DelayInGameScreen());
    }
    public bool IsShieldActive()
    {
        return hasShield;
    }

    private IEnumerator DelayInGameScreen()
    {
        yield return new WaitForSeconds(2);
        this.enabled = false;
        boxCollider2D.enabled = false;
        gameOverController.PlayerDied();
    }

}
