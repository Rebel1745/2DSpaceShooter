using UnityEngine;

public class E_EnemyShip1_IdleState : Enemy_IdleState
{
    private E_EnemyShip1 _enemyShip1;

    public E_EnemyShip1_IdleState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData, E_EnemyShip1 enemyShip1) : base(enemy, stateMachine, enemyData)
    {
        _enemyShip1 = enemyShip1;
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_enemy.CanAttack)
        {
            _stateMachine.ChangeState(_enemyShip1.AttackState);
        }
    }
}
