using UnityEngine;

public class Asteroid : Entity, IDamageable, IDestroyable, IEnemy
{
    [SerializeField] EnemySO _enemyData;

    protected override void Update()
    {
        base.Update();

        if (HasComeOnscreenYet)
        {
            if (_enemyData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_enemyData.DestroyWhenOffscreenDistance))
                QuietDestroy();
        }
    }

    public void DestroyObject()
    {
        WaveManager.Instance.EnemyDestroyed();
        if (_enemyData.DestructionParticles)
            ObjectPoolManager.SpawnObject(_enemyData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void QuietDestroy()
    {
        WaveManager.Instance.EnemyDestroyed();
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
}
