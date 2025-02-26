using UnityEngine;

public class Missile : MonoBehaviour, IDestroyable
{
    ProjectileSO _projectileData;

    public void SetupMissile(ProjectileSO projectile)
    {
        _projectileData = projectile;
        GetComponent<MoveForward>().SetSpeed(_projectileData.ProjectileSpeed);
        GetComponent<DestroyAfterTime>().SetDestroyAfterTime(_projectileData.TimeUntilProjectileDestroyed);
        gameObject.layer = LayerMask.NameToLayer("PlayerProjectiles");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(_projectileData.Damage);
            DestroyObject();
        }

    }

    public void DestroyObject()
    {
        if (_projectileData.DestructionParticles)
            Instantiate(_projectileData.DestructionParticles, transform.position, Quaternion.identity);

        // This is the function that should create an explosion and remove the missile from the pooled objects pool.
        // for now it will just destroy the object
        Destroy(gameObject);
    }
}
