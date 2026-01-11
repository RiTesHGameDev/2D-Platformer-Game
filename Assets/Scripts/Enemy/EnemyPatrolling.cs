using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyPatrolling : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private float patrolSpeed ;
    [SerializeField] private float edgeCooldown = 0.5f;

    [SerializeField] private GameObject groundDetector;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D boxCollider2D;

    private int directionChanger = 1;
    //private bool isFacingRight = true;
    //private float lastFlipTime;
    //private bool canFlip = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Start(){}
    private void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        animator.SetBool("IsPatrol", true);

        transform.Translate(directionChanger * Vector2.right * patrolSpeed * Time.deltaTime);

        Vector2 rayStart = boxCollider2D.bounds.center;
        rayStart.y -= boxCollider2D.bounds.extents.y;
        // Shoot ray slightly downward
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            rayDistance,
            groundLayer
        );

        if (!hit)
        {
            Vector3 scaleVector = transform.localScale;
            scaleVector.x *= -1;
            transform.localScale = scaleVector;
            directionChanger *= -1;
        }
        Debug.DrawRay(groundDetector.transform.position, Vector2.down * rayDistance, Color.red);

    }    
}