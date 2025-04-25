using UnityEngine;

public class E_EnemyShip1 : Enemy
{
    public E_EnemyShip1_IdleState IdleState { get; private set; }
    public E_EnemyShip1_MoveState MoveState { get; private set; }
    public E_EnemyShip1_AttackState AttackState { get; private set; }

    public override void InitialiseStates()
    {
        base.InitialiseStates();

        IdleState = new E_EnemyShip1_IdleState(this, _stateMachine, EnemyData, this);
        MoveState = new E_EnemyShip1_MoveState(this, _stateMachine, EnemyData, this);
        AttackState = new E_EnemyShip1_AttackState(this, _stateMachine, EnemyData, this);

        _stateMachine.Initialise(MoveState);
    }
}
