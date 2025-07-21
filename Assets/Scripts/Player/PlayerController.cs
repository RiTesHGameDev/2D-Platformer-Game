using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
    }
    void HandleJumpCrouch()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 boxsize = boxCollider2D.size;
        Vector2 boxOffset = boxCollider2D.offset;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch", true);
            boxOffset.x = -0.1f;
            boxOffset.y = 0.58f;
            boxsize.x = 0.6f;
            boxsize.y = 1.28f;
        }
        else 
        {
            animator.SetBool("Crouch", false);
            boxOffset.x = 0f;
            boxOffset.y = 0.92f;
            boxsize.x = 0.5f;
            boxsize.y = 1.92f;
        }
        boxCollider2D.size = boxsize;
        boxCollider2D.offset = boxOffset;

        if (verticalInput > 0 || Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
            boxOffset.x = 0f;
            boxOffset.y = 1.68f;
            boxsize.x = 0.5f;
            boxsize.y = 1.3f;
        }
        else
        {
            animator.SetBool("Jump", false);
            boxOffset.x = 0f;
            boxOffset.y = 0.92f;
            boxsize.x = 0.5f;
            boxsize.y = 1.92f;
        }

    }
       void HandleIdleWalkRun()
    {
        float speed = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(speed));

        Vector3 scale = transform.localScale;
        if (speed < 0f)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (speed > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        HandleIdleWalkRun();
        HandleJumpCrouch();
    }
}
