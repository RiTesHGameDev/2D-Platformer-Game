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

    private Vector2 originalBoxSize;
    private Vector2 originalBoxOffset;
    private Rigidbody2D rb2D;
    private bool isGrounded;

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
        UpdateAnimations(horizontal);
    }

    private void CheckGrounded()
    {
        //// Check if player is grounded using a raycast
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,
        //                    boxCollider2D.bounds.extents.y + groundCheckDistance, groundLayer);
        //isGrounded = hit.collider != null;

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

        // Jump if grounded and jump input is pressed
        if (jumpInput && isGrounded)
        {
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

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

    private void UpdateAnimations(float horizontal)
    {
        // Run animation
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        // Flip character sprite based on movement direction
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontal);
            transform.localScale = scale;
        }

        // Jump animation
        animator.SetBool("Jump", !isGrounded);
    }

    public void PickUpKey()
    {
        Debug.Log("Player picked up the key");
        scoreController.IncreaseScore(10);
    }

    public void PlayerDeath()
    {
        Debug.Log("PLayer Death by Enemy");
        animator.SetBool("Death",true);
        gameOverController.PlayerDied();
        this.enabled = false;
    }

}
