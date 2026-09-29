using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    private float knockbackDuration;
    private Vector2 knockbackDir;
    private float knockbackForce;
    private float timer;
    public EnemyHurtState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }
    public override void Enter()
    {
        enemy.currentStateDebug = Enemy.EnemyStateDebug.Hurt;
        timer = knockbackDuration;
        enemy.isHurting = true;
        enemy.animator.SetTrigger("Hurt");
        enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
        enemy.rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        enemy.isKnockedBack = true;
    }
    public override void Execute()
    {
        if (enemy.isDead)
        {
            //Change dead state
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            enemy.DetectPlayer();
            if (enemy.canSeePlayer)
            {
                stateMachine.ChangeState(enemy.ChaseState);
            }
            else
            {
                stateMachine.ChangeState(enemy.PatrolState);
            }
        }
    }
    public override void Exit()
    {
        enemy.rb.velocity = new Vector2(0f,enemy.rb.velocity.y);
        timer = 0f;
        knockbackDuration = 0f;
        knockbackForce = 0f;
        enemy.isHurting = false;
        enemy.isKnockedBack = false;
    }
    public void SetKnockbackData(Vector2 dir,float force, float duration)
    {
        knockbackDir = dir;
        knockbackForce = force;
        knockbackDuration = duration;
    }
}
