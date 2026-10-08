using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class EnemyBaseState : IState
{
    protected Enemy enemy;
    protected StateMachine stateMachine;
    public EnemyBaseState(Enemy enemy, StateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine; 
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Execute();
}
