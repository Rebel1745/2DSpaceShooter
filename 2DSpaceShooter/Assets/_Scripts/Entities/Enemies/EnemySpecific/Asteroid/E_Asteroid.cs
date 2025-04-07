using UnityEngine;
using UnityEngine.Splines;

public class E_Asteroid : Enemy
{
    public E_Asteroid_MoveState MoveState { get; private set; }

    protected override void Update()
    {
        base.Update();

        if (!_splineAnimationFinished)
        {
            CheckSplineAnimationFinished();
        }
        else QuietDestroy();
    }

    public override void TakeDamage(int amount)
    {
        DestroyObject();
    }

    public override void InitialiseStates()
    {
        base.InitialiseStates();

        MoveState = new E_Asteroid_MoveState(this, _stateMachine, EnemyData);
        _stateMachine.Initialise(MoveState);
    }
}
