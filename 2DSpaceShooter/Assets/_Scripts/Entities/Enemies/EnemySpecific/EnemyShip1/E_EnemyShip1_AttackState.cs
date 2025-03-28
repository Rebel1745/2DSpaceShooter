using UnityEngine;

public class E_EnemyShip1_AttackState : Enemy_AttackState
{
    private E_EnemyShip1 _enemyShip1;

    public E_EnemyShip1_AttackState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData, E_EnemyShip1 enemyShip1) : base(enemy, stateMachine, enemyData)
    {
        _enemyShip1 = enemyShip1;
    }
}
