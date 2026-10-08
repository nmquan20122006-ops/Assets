using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Profiling;
using UnityEngine;
public enum EnemyStateDebug { Patrol, Chase, Attack, Hurt }
public class Enemy : Character
{
    [field: SerializeField] public float speed { get; private set; }
    public bool canSeePlayer { get; private set; }
    public float distanceToPlayer => DistanceBtPlayer();
    public bool isInAttackRange => IsInAttackRange();
    [field: SerializeField] public int visionRange {  get; private set; }
    public Vector2 facingDirection { get; protected set; }
    public bool isWaiting {  get; set; }
    public bool isHurting {  get; set; }
    public Transform playerTransform { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    private SpriteRenderer spriteRenderer;
    [field: SerializeField] public EnemyStateDebug currentStateDebug { get; set; }
    [field: SerializeField] public float attackRange { get; private set; }
    public EnemyAttack enemyAttack { get; private set; }
    public bool isKnockedBack {  get; set; }
    protected Vector2 _knockbackDir;
    protected float _knockbackDuration;
    protected float _knockbackForce;

    //base Enemy State
    public StateMachine stateMachine { get; private set; }
    public EnemyPatrolState patrolState;
    public EnemyAttackState attackState;
    public EnemyChaseState chaseState;
    public EnemyHurtState hurtState;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();
        patrolState = new EnemyPatrolState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        hurtState = new EnemyHurtState(this, stateMachine);

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
    }
    protected override void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }
    void Update()
    {
        Debug.DrawRay(transform.position, facingDirection * visionRange, Color.red);
        DetectPlayer();
    }
    protected virtual void FixedUpdate()
    {
        stateMachine.Update();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
    public void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return;

        if (directionX > 0.01f)
        {
            spriteRenderer.flipX = false;
            facingDirection = Vector2.right;
        }
        else if (directionX < -0.01f)
        {
            spriteRenderer.flipX = true;
            facingDirection = Vector2.left;
        }
    }
    private void DetectPlayer()
    {
        float distance = DistanceBtPlayer();
        if (distance <= visionRange)
        {
            Vector2 toPlayer = (Vector2)playerTransform.position - (Vector2)rb.position;
            if (facingDirection.x > 0 && toPlayer.x > 0 || facingDirection.x < 0 && toPlayer.x < 0)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }
    private float DistanceBtPlayer()
    {
        if (rb == null)
        {
            return Mathf.Infinity;
        }
        return Vector2.Distance(rb.position, playerTransform.position);
    }
    public override void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector2.zero, 0f, 0f, 0f);
    }
    public virtual void TakeDamage(int damage, Vector2 attackerPosition, float lift, float force, float duration)
    {
        base.TakeDamage(damage);
        float dirX = Mathf.Sign(transform.position.x - attackerPosition.x);
        Vector2 knockbackDir = new Vector2(dirX, lift).normalized;
        SetKnockbackData(knockbackDir, force, duration);
        stateMachine.ChangeState(hurtState);
    }
    public void InverseFacingDir()
    {
        facingDirection = new Vector2(-facingDirection.x, facingDirection.y);
    }
    private bool IsInAttackRange()
    {
        if(DistanceBtPlayer() <= attackRange)
        {
            return true;
        }
        return false;
    }
    public void SetKnockbackData(Vector2 dir, float force, float duration)
    {
        _knockbackDir = dir;
        _knockbackForce = force;
        _knockbackDuration = duration;
    }
    public virtual void Patrol() { }
    public virtual void Chase()
    { 
        if(playerTransform == null)
        {
            stateMachine.ChangeState(patrolState);
        }
        float dirX = Mathf.Sign(playerTransform.position.x - rb.position.x);
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        FlipSprite(dirX);
        if (isInAttackRange)
        {
            //changeState to attack
        }
        if (!canSeePlayer)
        {
            stateMachine.ChangeState(patrolState);
        }
    }
    public virtual void Attack()
    {
    }
    public virtual void Hurt()
    {
    }
    public virtual void EnterPatrol()
    {
        currentStateDebug = EnemyStateDebug.Patrol;
    }
    public virtual void ExitPatrol()
    {
    }
    public virtual void EnterChase()
    {
        currentStateDebug = EnemyStateDebug.Chase;
    }
    public virtual void ExitChase()
    {
    }
    public virtual void EnterAttack()
    {
        currentStateDebug = EnemyStateDebug.Attack;
    }
    public virtual void ExitAttack()
    {
    }
    public virtual void EnterHurt()
    {
        currentStateDebug = EnemyStateDebug.Hurt;
    }
    public virtual void ExitHurt()
    {
    }
}
