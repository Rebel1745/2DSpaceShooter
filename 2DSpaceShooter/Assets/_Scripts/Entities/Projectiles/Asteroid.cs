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
        if (_enemyData.DestructionParticles)
            Instantiate(_enemyData.DestructionParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void QuietDestroy()
    {
        Destroy(gameObject);
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
