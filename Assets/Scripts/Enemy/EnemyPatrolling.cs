using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private float patrolSpeed ;
    [SerializeField]private Vector3 pointA;
    [SerializeField]private Vector3 pointB;

    private Vector3 targetPoint;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() => SetPatrolPoints();
    private void Update()
    {
        Patrol();
    }
    private void SetPatrolPoints()
    {
        transform.position = pointA;
        targetPoint = pointB;
    }
    private void Patrol()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSpeed * Time.deltaTime);
        if (transform.position == targetPoint)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            FlipSprite();
        }
    }
    private void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}