using UnityEngine;

public class E_EnemyShip1_MoveState : Enemy_MoveState
{
    private E_EnemyShip1 _enemyShip1;

    public E_EnemyShip1_MoveState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData, E_EnemyShip1 e_EnemyShip1) : base(enemy, stateMachine, enemyData)
    {
        _enemyShip1 = e_EnemyShip1;
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
