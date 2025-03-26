using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // get the enemy moving
        _enemy._sa.Play();
    }
}
