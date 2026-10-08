using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) {}
    public override void Enter()
    {
        enemy.EnterAttack();
    }
    public override void Execute()
    {
        enemy.Attack();
        if (!enemy.canSeePlayer) { stateMachine.ChangeState(enemy.patrolState); return; }
        if (!enemy.isInAttackRange) { stateMachine.ChangeState(enemy.chaseState); return; }

    }
    public override void Exit()
    {
        enemy.ExitAttack();
    }
}