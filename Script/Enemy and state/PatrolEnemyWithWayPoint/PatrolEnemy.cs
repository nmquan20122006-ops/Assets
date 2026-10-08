using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
public class PatrolEnemy : Enemy
{
    [field: SerializeField] public Transform[] waypoints { get; private set; }
    [field: SerializeField] public float waypointWayTime { get; private set; }
    public int currentWaypointIndex { get; set; }
    private float _waitTimer;
    private float _knockbackTimer;
    private float _cooldownTimer;
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(patrolState);
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public override void EnterPatrol()
    {
        Debug.Log("EnterPatrol");
        _waitTimer = 0f;
        isWaiting = false;
        if (waypoints == null || waypoints.Length == 0) return;
        SetDirectionToWayPoint();
    }
    public override void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) { StopMoving(); return; }
        if (isWaiting) { HandleWaiting();return; }
        HandlePatrolMovement();
    }
    public override void ExitPatrol()
    {
        StopMoving();
    }
    private void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    private void StartWaiting()
    {
        isWaiting = true;
        _waitTimer = 0f;
        StopMoving();
    }
    private void HandlePatrolMovement()
    {
        Transform waypoint = waypoints[currentWaypointIndex];
        float dirX = Mathf.Sign(waypoint.position.x - transform.position.x);
        float distanceX = Mathf.Abs(waypoint.position.x - transform.position.x);
        if (distanceX < 0.2f)
        {
            StartWaiting();
            return;
        }
        Move(dirX);
    }
    private void SetDirectionToWayPoint()
    {
        Transform waypoint = waypoints[currentWaypointIndex];
        float dirX = Mathf.Sign(waypoint.position.x - transform.position.x);
        FlipSprite(dirX);
    }
    private void HandleWaiting()
    {
        StopMoving();
        _waitTimer += Time.deltaTime;
        if (_waitTimer >= waypointWayTime)
        {
            isWaiting = false;
            _waitTimer = 0f;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDirectionToWayPoint();
        }
    }
    private void Move(float dirX)
    {
        rb.velocity = new Vector2(speed * dirX, rb.velocity.y);
        FlipSprite(dirX);
    }
    public override void EnterChase()
    {
        base.EnterChase();
    }
    public override void Chase()
    {
        base.Chase();
    }
    public override void ExitChase()
    {
        StopMoving();
    }
    public override void EnterHurt()
    {
        base.EnterHurt();
        _knockbackTimer = _knockbackDuration;
        isHurting = true;
        animator.SetTrigger("Hurt");
        rb.AddForce(_knockbackDir * _knockbackForce, ForceMode2D.Impulse);
        isKnockedBack = true;
    }
    public override void Hurt()
    {
        _knockbackTimer -= Time.deltaTime;
    }
    public override void ExitHurt()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        _knockbackTimer = 0f;
        _knockbackDuration = 0f;
        _knockbackForce = 0f;
        isHurting = false;
        isKnockedBack = false;
    }
    public override void EnterAttack()
    {
        base.EnterAttack();
        _cooldownTimer = enemyAttack.attackCooldown;
        Debug.Log(_cooldownTimer);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        float dirX = Mathf.Sign(playerTransform.position.x - rb.position.x);
        FlipSprite(dirX);
        enemyAttack.AttackLogic();
    }
    public override void Attack()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (!enemyAttack.isAttackFinished) return;
        _cooldownTimer -= Time.deltaTime;
        if (isInAttackRange && _cooldownTimer <= 0)
        {
            enemyAttack.AttackLogic();
            _cooldownTimer = enemyAttack.attackCooldown;
        }
    }
    public override void ExitAttack()
    {
        enemyAttack.isAttackFinished = false;
    }
}
