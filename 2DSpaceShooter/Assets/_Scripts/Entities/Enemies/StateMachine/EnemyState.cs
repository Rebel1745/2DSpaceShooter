using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;
    protected EnemySO _enemyData;

    protected float _startTime;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _enemyData = enemyData;
    }

    public virtual void Enter()
    {
        DoChecks();
        _startTime = Time.time;
    }

    public virtual void Exit() { }

    public virtual void LogicUpate() { }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }
}
