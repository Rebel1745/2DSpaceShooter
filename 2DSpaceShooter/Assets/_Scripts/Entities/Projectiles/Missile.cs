using UnityEngine;

public class Missile : Entity, IDestroyable
{
    WeaponProjectile _projectileData;

    protected override void Update()
    {
        base.Update();

        if (HasComeOnscreenYet)
        {
            if (_projectileData.DestroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_projectileData.DestroyWhenOffscreenDistance))
                QuietDestroy();
        }
    }

    public void SetupMissile(WeaponProjectile projectile)
    {
        _projectileData = projectile;
        GetComponent<MoveForward>().SetSpeed(_projectileData.ProjectileSpeed);
        gameObject.layer = _projectileData.IsPlayerProjectile ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("EnemyProjectile");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasComeOnscreenYet)
            return;

        if (collision.transform.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(_projectileData.Damage);

            if (_projectileData.DestroyOnContact)
                DestroyObject();
        }
    }

    public void DestroyObject()
    {
        if (_projectileData.DestructionParticles)
            ObjectPoolManager.SpawnObject(_projectileData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    // destry the object without creating an explosion or any palava (suitable for going offscreen)
    public void QuietDestroy()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
