using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public BoxCollider2D boxCollider2D;

    [SerializeField] public float speed;
    [SerializeField] public float jump;

    private Vector2 boxSize;
    private Vector2 boxOffset;
    private Rigidbody2D rb2D;
    private void Awake()
    {
        Debug.Log("Player Controller awake");
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //Storing Collider Original Values
        boxSize = boxCollider2D.size;
        boxOffset = boxCollider2D.offset;
    }
    private void Crouch(bool crouch)
    {
        if (crouch == true)
        {
            float offX = -0.1f;
            float offY = 0.58f;
            float sizeX = 0.6f;
            float sizeY = 1.28f;

            boxCollider2D.size = new Vector2 (sizeX,sizeY);
            boxCollider2D.offset = new Vector2 (offX, offY);
        }
        else 
        {
            boxCollider2D.size = boxSize;
            boxCollider2D.offset = boxOffset;
        }
        animator.SetBool("Crouch", crouch);
    }
    private void PlayeMovementAnimation(float horizonal , float vertical)
    {
        //Run
        animator.SetFloat("Speed", Mathf.Abs(horizonal));

        Vector3 scale = transform.localScale;
        if (horizonal < 0f)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (horizonal > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        //Jump
        if (vertical > 0)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }
    private void MoveCharacter(float horizontal,float vertical)
    {
        //Move Character Horizontally
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        transform.position = position;

        //Move Character Vertically
        if (vertical > 0)
        {
            rb2D.AddForce(new Vector2(0f, jump), ForceMode2D.Force);
        }
    }
    void Update()
    {
        float horizonal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Jump");

        PlayeMovementAnimation(horizonal,vertical);
        MoveCharacter(horizonal,vertical);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }
    }
}
