using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    public EnemyHurtState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        Debug.Log("Enter hurt");
        enemy.EnterHurt();
    }
    public override void Execute()
    {
        enemy.Hurt();
        if (enemy.canSeePlayer)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
        else
        {
            stateMachine.ChangeState(enemy.patrolState);
        }
    }
    public override void Exit()
    {
        enemy.ExitHurt();
    }
}
