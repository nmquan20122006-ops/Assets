using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
public class EnemyPatrolState : EnemyBaseState
{
    private float waitTimer = 0f;
    public EnemyPatrolState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }
    public override void Enter()
    {
        enemy.currentStateDebug = Enemy.EnemyStateDebug.Patrol;
        waitTimer = 0f;

        if (enemy.waypoints == null || enemy.waypoints.Length == 0) return;
        Vector2 target = enemy.waypoints[enemy.currentWayPointIndex].position;
        float dirX = Mathf.Sign(target.x - enemy.rb.position.x);
        enemy.FlipSprite(dirX);
    }
    public override void Execute()
    {
        enemy.DetectPlayer();
        //waiting
        if (enemy.isWaiting)
        {
            if (enemy.canSeePlayer)
            {
                stateMachine.ChangeState(enemy.ChaseState);
                return;
            }
            waitTimer += Time.deltaTime;
            enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);

            if (waitTimer >= enemy.waypointWaitTime)
            {
                enemy.isWaiting = false;
                waitTimer = 0f;
                enemy.currentWayPointIndex = (enemy.currentWayPointIndex + 1) % enemy.waypoints.Length;
            }
        }
        //moving
        else
        {
            if (enemy.canSeePlayer)
            {
                stateMachine.ChangeState(enemy.ChaseState);
                return;
            }
            if (enemy.waypoints.Length == 0 || enemy.waypoints == null) return;
            Vector2 target = enemy.waypoints[enemy.currentWayPointIndex].transform.position;
            Vector2 current = enemy.transform.position;

            float dirX = Mathf.Sign(target.x - current.x);
            float distanceX = Mathf.Abs(target.x - current.x);
            //reach the way point
            if (distanceX < 0.2f)
            {
                enemy.isWaiting = true;

            }
            // not reach the way point
            else
            {
                enemy.rb.velocity = new Vector2(dirX * enemy.speed, enemy.rb.velocity.y);
                enemy.FlipSprite(dirX);
            }
        }
    }
    public override void Exit()
    {
        enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
    }
}