using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public Enemy_IdleState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData) : base(enemy, stateMachine, enemyData)
    {
    }
}
