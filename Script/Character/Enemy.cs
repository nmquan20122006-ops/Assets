using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Profiling;
using UnityEngine;
public class Enemy : Character
{
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public Transform[] waypoints { get; private set; }
    [field: SerializeField] public float waypointWaitTime { get; private set; }
    [field: SerializeField] public int currentWayPointIndex {  get; set; }
    [field: SerializeField] public bool canSeePlayer { get; private set; }
    [field: SerializeField] public int visionRange {  get; private set; }
    [field: SerializeField] public Vector2 facingDirection { get; private set; }
    [field: SerializeField] public bool isWaiting {  get; set; }
    [field: SerializeField] public bool isHurting {  get; set; }
    public Transform playerTranform { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

    private SpriteRenderer spriteRenderer;
    public enum EnemyStateDebug { Patrol, Chase, Attack, Hurt }
    [field: SerializeField] public EnemyStateDebug currentStateDebug { get; set; }
    [field: SerializeField] public float attackRange { get; private set; }
    public EnemyAttack enemyAttack { get; private set; }
    [field: SerializeField] public bool isKnockedBack {  get; set; }
    public StateMachine stateMachine { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState {  get; private set; }
    public EnemyHurtState HurtState { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
        stateMachine = new StateMachine();
        PatrolState = new EnemyPatrolState(this, stateMachine);
        ChaseState = new EnemyChaseState(this, stateMachine);
        AttackState = new EnemyAttackState(this, stateMachine);
        HurtState = new EnemyHurtState(this, stateMachine);
    }
    protected override void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTranform = playerObj.transform;
        }
        stateMachine.Initialize(PatrolState);
    }
    void Update()
    {
        Debug.DrawRay(transform.position, facingDirection * visionRange, Color.red);
    }
    private void FixedUpdate()
    {
        facingDirection = ((Vector2)waypoints[currentWayPointIndex].position - rb.position).normalized;
        stateMachine.Update();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
    public void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return;

        if (directionX > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (directionX < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }
    public void DetectPlayer()
    {
        float distance = DistanceBtPlayer();
        if (distance <= visionRange)
        {
            Vector2 toPlayer = (Vector2)playerTranform.position - (Vector2)rb.position;
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
    public float DistanceBtPlayer()
    {
        return Vector2.Distance(rb.position, playerTranform.position);
    }
    public override void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector2.zero, 0f, 0f, 0f);
    }
    public void TakeDamage(int damage, Vector2 attackerPosition, float lift, float force, float duration)
    {
        base.TakeDamage(damage);
        if (isDead) { Die(); return; }
        float dirX = Mathf.Sign(transform.position.x - attackerPosition.x);
        Vector2 knockbackDir = new Vector2(dirX, lift).normalized;
        HurtState.SetKnockbackData(knockbackDir, force, duration);
        stateMachine.ChangeState(HurtState);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, currentStateDebug.ToString());
    }
#endif
}
