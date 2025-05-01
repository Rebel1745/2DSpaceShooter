using UnityEngine;

public class Bomb : Entity, IDestroyable
{
    private WeaponBomb _bombData;
    private float _bombSpawnTime;
    private Collider2D[] _enemyColliders;

    protected override void Update()
    {
        base.Update();

        if (HasComeOnscreenYet)
        {
            if (_bombData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_bombData.DestroyWhenOffscreenDistance))
                QuietDestroy();
        }
    }

    public void SetupBomb(WeaponBomb bombData)
    {
        _bombData = bombData;
        GetComponent<MoveForward>().SetSpeed(_bombData.ProjectileSpeed);
        gameObject.layer = _bombData.IsPlayerProjectile ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("EnemyProjectile");
        _bombSpawnTime = Time.time;
    }

    public bool ExplodeBomb()
    {
        // find all of the objects inside the blast radius
        _enemyColliders = Physics2D.OverlapCircleAll(transform.position, _bombData.ExplosionRadius, _bombData.WhatIsEnemy);

        if (_enemyColliders.Length > 0)
        {
            foreach (Collider2D collider in _enemyColliders)
            {
                if (collider.transform.TryGetComponent<IDamageable>(out IDamageable obj))
                {
                    obj.TakeDamage(_bombData.Damage);
                }
            }
        }

        // time to explode
        DestroyObject();

        return true;
    }

    public void DestroyObject()
    {
        _bombData.MarkBombExploded();

        if (_bombData.DestructionParticles)
            ObjectPoolManager.SpawnObject(_bombData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void QuietDestroy()
    {
        _bombData.MarkBombExploded();

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
