using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData) : base(enemy, stateMachine, enemyData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // get the enemy moving
        if (!_enemy.SA.IsPlaying)
            _enemy.SA.Restart(true);
    }
}
