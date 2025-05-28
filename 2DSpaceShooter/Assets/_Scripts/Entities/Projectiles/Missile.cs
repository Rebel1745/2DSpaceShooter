using UnityEngine;

public class Missile : Entity, IDestroyable
{
    private float _destroyWhenOffscreenDistance = 0f;
    private float _projectileSpeed;
    private float _damage;
    private bool _destroyOnContact = true;
    private GameObject _destructionParticles;

    protected override void Update()
    {
        base.Update();

        if (HasComeOnscreenYet)
        {
            if (_destroyWhenOffscreenDistance != 0f && CheckIfOffscreenByAmount(_destroyWhenOffscreenDistance))
                QuietDestroy();
        }

        transform.Translate(0f, _projectileSpeed * Time.deltaTime, 0f);
    }

    public void SetupMissile(WeaponProjectile projectile)
    {
        _destroyWhenOffscreenDistance = projectile.DestroyWhenOffscreenDistance;
        _projectileSpeed = projectile.ProjectileSpeed;
        gameObject.layer = projectile.IsPlayerProjectile ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("EnemyProjectile");
        _damage = projectile.Damage;
        _destroyOnContact = projectile.DestroyOnContact;
        _destructionParticles = projectile.DestructionParticles;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasComeOnscreenYet)
            return;

        if (collision.transform.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(_damage);

            if (_destroyOnContact)
                DestroyObject();
        }
    }

    public void DestroyObject()
    {
        if (_destructionParticles)
            ObjectPoolManager.SpawnObject(_destructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    // destry the object without creating an explosion or any palava (suitable for going offscreen)
    public void QuietDestroy()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
