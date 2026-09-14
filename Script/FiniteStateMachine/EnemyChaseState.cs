using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyChaseState : EnemyBaseState
{
    private Transform playerTransform;
    private float dirX = 0f;
    public EnemyChaseState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }
    public override void Enter()
    {
        enemy.currentStateDebug = Enemy.EnemyStateDebug.Chase;
        playerTransform = enemy.playerTranform;
    }
    public override void Execute()
    {
        enemy.DetectPlayer();
        if (playerTransform == null)
        {
            //TODO: change enemy idle state
            stateMachine.ChangeState(enemy.PatrolState);
        }
        //moving forward to player
        Vector2 target = playerTransform.position - enemy.transform.position;
        dirX = Mathf.Sign(playerTransform.position.x - enemy.rb.position.x);
        enemy.rb.velocity = new Vector2(dirX * enemy.speed, enemy.rb.velocity.y);
        enemy.FlipSprite(dirX);
        //Transition
        float distance = enemy.DistanceBtPlayer();
        if (distance <= enemy.attackRange)
        {
            //TODO: change enemy attack state
            stateMachine.ChangeState(enemy.AttackState);
        }
        if (!(enemy.canSeePlayer))
        {
            //TODO: change enemy patrol state
            stateMachine.ChangeState(enemy.PatrolState);
        }
    }
    public override void Exit()
    {
        enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
    }
}
