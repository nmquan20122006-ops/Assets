public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) 
    {
    }
    public override void Enter()
    {
        enemy.currentStateDebug = EnemyStateDebug.Patrol;
        enemy.EnterPatrol();
    }
    public override void Execute()
    {
        enemy.Patrol();
        if (enemy.canSeePlayer) { stateMachine.ChangeState(enemy.chaseState); return; }
    }
    public override void Exit()
    {
        enemy.ExitPatrol();
    }
}