using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData) : base(enemy, stateMachine, enemyData)
    {
    }
}
