using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttackState : EnemyBaseState
{
    private Transform playerTransform;
    private float dirX;
    private float distanceToPlayer;
    private float cooldownTimer;
    public EnemyAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }
    public override void Enter()
    {
        //init
        enemy.currentStateDebug = Enemy.EnemyStateDebug.Attack;
        cooldownTimer = enemy.enemyAttack.attackCooldown;
        playerTransform = enemy.playerTranform;
        //reset enemy velocity and flip sprite f2f player
        enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
        
        dirX = Mathf.Sign(playerTransform.position.x - enemy.transform.position.x);
        enemy.FlipSprite(dirX);
        //attack
        enemy.enemyAttack.AttackLogic();
    }
    public override void Execute()
    {
        //detect and distance updating every frame
        enemy.DetectPlayer();
        distanceToPlayer = enemy.DistanceBtPlayer();
        
        enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);

        //attack didnt finish -> wait
        if (!enemy.enemyAttack.isAttackFinished) return;
        //attack finished -> change state
        cooldownTimer -= Time.deltaTime;
        if (!enemy.canSeePlayer)
        {
            stateMachine.ChangeState(enemy.PatrolState);

        }
        //player out attack range -> chase
        else if (distanceToPlayer > enemy.attackRange)
        {
            stateMachine.ChangeState(enemy.ChaseState);

        }
        //player in attack range -> attack
        else if (distanceToPlayer <= enemy.attackRange && cooldownTimer <= 0)
        {
            enemy.enemyAttack.AttackLogic();
            cooldownTimer = enemy.enemyAttack.attackCooldown;
        }
    }
    public override void Exit()
    {
        enemy.enemyAttack.isAttackFinished = false;
    }
}