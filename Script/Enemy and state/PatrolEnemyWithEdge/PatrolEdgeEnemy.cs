using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PatrolEdgeEnemy : Enemy
{
    private EnemyChaseEdgeState _enemyChaseEdgeState;
    private EnemyPatrolEdgeState _enemyPatrolEdgeState;
    private LedgeAndWallDetector _ledgeAndWallDetector;
    [SerializeField] private float _waitTime = 0f;
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(_enemyPatrolEdgeState);
    }
    protected override void Awake()
    {
        base.Awake();
        _enemyChaseEdgeState = new EnemyChaseEdgeState(this, stateMachine);
        _enemyPatrolEdgeState = new EnemyPatrolEdgeState(this, stateMachine);
        _ledgeAndWallDetector = GetComponent<LedgeAndWallDetector>();
    }
    private class EnemyPatrolEdgeState : EnemyBaseState
    {
        private PatrolEdgeEnemy _patrolEdgeEnemy;
        private float _waitTimer;
        public EnemyPatrolEdgeState(PatrolEdgeEnemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
            this._patrolEdgeEnemy = enemy;
        }

        public override void Enter()
        {
            enemy.currentStateDebug = EnemyStateDebug.Patrol;
            _waitTimer = 0f;
            float facingDirectionX = enemy.facingDirection.x;
            enemy.FlipSprite(facingDirectionX);
        }

        public override void Execute()
        {
            float dirX = enemy.facingDirection.x;

            if (enemy.isWaiting)
            {
                if (enemy.canSeePlayer)
                {
                    stateMachine.ChangeState(_patrolEdgeEnemy._enemyChaseEdgeState);
                    return;
                }
                _waitTimer += Time.deltaTime;
                enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
                if (_waitTimer >= _patrolEdgeEnemy._waitTime)
                {
                    enemy.isWaiting = false;
                    _waitTimer = 0f;
                    enemy.InverseFacingDir();
                }
            }
            else
            {
                if (enemy.canSeePlayer)
                {
                    stateMachine.ChangeState(_patrolEdgeEnemy._enemyChaseEdgeState);
                    return;
                }
                
                if (_patrolEdgeEnemy._ledgeAndWallDetector.IsHittingWall(dirX)
                    || _patrolEdgeEnemy._ledgeAndWallDetector.IsAtEdge(dirX))
                {
                    enemy.isWaiting = true;
                    enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
                    return;
                }
                enemy.rb.velocity = new Vector2(dirX * enemy.speed, enemy.rb.velocity.y);
                enemy.FlipSprite(dirX);
            }
        }
        public override void Exit()
        {
            enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
        }
    }
    private class EnemyChaseEdgeState : EnemyBaseState
    {
        private PatrolEdgeEnemy _patrolEdgeEnemy;
        private Transform _playerTransform;
        public EnemyChaseEdgeState(PatrolEdgeEnemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
        {
            this._patrolEdgeEnemy = enemy;
        }

        public override void Enter()
        {
            enemy.currentStateDebug = EnemyStateDebug.Chase;
            _playerTransform = enemy.playerTransform;
        }

        public override void Execute()
        {
            if (_playerTransform == null) stateMachine.ChangeState(_patrolEdgeEnemy._enemyPatrolEdgeState);

            Vector2 target = _playerTransform.position - enemy.transform.position;
            float dirX = Mathf.Sign(_playerTransform.position.x - enemy.rb.position.x);
            enemy.rb.velocity = new Vector2(dirX * enemy.speed, enemy.rb.velocity.y);
            enemy.FlipSprite(dirX);
            //Transition
            float distance = enemy.distanceToPlayer;
            if (distance <= enemy.attackRange)
            {
                //TODO: change enemy attack state
                
            }
            if (!(enemy.canSeePlayer))
            {
                stateMachine.ChangeState(_patrolEdgeEnemy._enemyPatrolEdgeState);
                
            }
        }

        public override void Exit()
        {
            enemy.rb.velocity = new Vector2(0f, enemy.rb.velocity.y);
        }
    }
}
