using UnityEngine;

public class E_EnemyShip1 : Enemy, IDamageable, IDestroyable
{
    public E_EnemyShip1_MoveState MoveState { get; private set; }

    public void DestroyObject()
    {
        throw new System.NotImplementedException();
    }

    public void QuietDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();

        MoveState = new E_EnemyShip1_MoveState(this, _stateMachine);
    }

    private void Start()
    {
        _stateMachine.Initialise(MoveState);
    }
}
