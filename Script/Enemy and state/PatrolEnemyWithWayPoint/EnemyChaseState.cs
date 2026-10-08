public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        enemy.currentStateDebug = EnemyStateDebug.Chase;
        enemy.EnterChase();
    }
    public override void Execute()
    {
        if (!enemy.canSeePlayer) { stateMachine.ChangeState(enemy.patrolState); return; }
        if (enemy.isInAttackRange)
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }
        enemy.Chase();
    }
    public override void Exit()
    {
        enemy.ExitChase();
    }
}