using UnityEngine;
using UnityEngine.Splines;

public class E_Asteroid : Enemy, IDamageable, IDestroyable
{
    public E_Asteroid_MoveState MoveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        MoveState = new E_Asteroid_MoveState(this, _stateMachine);
    }

    private void Start()
    {
        _stateMachine.Initialise(MoveState);
    }

    protected override void Update()
    {
        base.Update();

        if (_hasComeOnscreenYet)
        {
            if (_enemyData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_enemyData.DestroyWhenOffscreenDistance))
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
        if (_enemyData.DestructionParticles)
            ObjectPoolManager.SpawnObject(_enemyData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(_sa.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void QuietDestroy()
    {
        WaveManager.Instance.EnemyDestroyed();
        ObjectPoolManager.ReturnObjectToPool(_sa.Container.gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void TakeDamage(int amount)
    {
        DestroyObject();
    }
}
