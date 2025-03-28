using UnityEngine;
using UnityEngine.Splines;

public class E_Asteroid : Enemy, IDamageable, IDestroyable
{
    public E_Asteroid_MoveState MoveState { get; private set; }

    protected override void Update()
    {
        base.Update();

        if (_hasComeOnscreenYet)
        {
            if (EnemyData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(EnemyData.DestroyWhenOffscreenDistance))
                QuietDestroy();
        }

        if (!_splineAnimationFinished)
        {
            CheckSplineAnimationFinished();
        }
        else QuietDestroy();
    }

    public void DestroyObject()
    {
        WaveManager.Instance.EnemyDestroyed();
        if (EnemyData.DestructionParticles)
            ObjectPoolManager.SpawnObject(EnemyData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(SA.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void QuietDestroy()
    {
        WaveManager.Instance.EnemyDestroyed();
        ObjectPoolManager.ReturnObjectToPool(SA.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void TakeDamage(int amount)
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
