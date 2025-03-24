using UnityEngine;
using UnityEngine.Splines;

public class Asteroid : Entity, IDamageable, IDestroyable, IEnemy
{
    [SerializeField] EnemySO _enemyData;
    private SplineAnimate _sa;
    private Vector2 _enemyGridPosition;
    private bool _splineAnimationFinished;

    private void Awake()
    {
        _sa = GetComponent<SplineAnimate>();
    }

    protected override void Update()
    {
        base.Update();

        if (HasComeOnscreenYet)
        {
            if (_enemyData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_enemyData.DestroyWhenOffscreenDistance))
                QuietDestroy();
        }

        // unused for now, might want to keep for later
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

    public void SetEnemyData(EnemySO enemyData)
    {
        _enemyData = enemyData;
    }

    public void TakeDamage(int amount)
    {
        DestroyObject();
    }

    public void SetGridPosition(Vector2 pos)
    {
        _enemyGridPosition = pos;
    }

    public void CheckSplineAnimationFinished()
    {
        _splineAnimationFinished = !_sa.IsPlaying;
    }
}
