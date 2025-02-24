using UnityEngine;

public class Missile : MonoBehaviour
{
    ProjectileSO _projectile;

    public void SetupMissile(ProjectileSO projectile)
    {
        _projectile = projectile;
        GetComponent<MoveForward>().SetSpeed(_projectile.ProjectileSpeed);
        GetComponent<DestroyAfterTime>().SetDestroyAfterTime(_projectile.TimeUntilProjectileDestroyed);
        gameObject.layer = LayerMask.NameToLayer("PlayerProjectiles");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided");
        if (collision.transform.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.TakeDamage(_projectile.Damage);
            Destroy();
        }

    }

    public void Destroy()
    {
        // This is the function that should create an explosion and remove the missile from the pooled objects pool.
        // for now it will just destroy the object
        Destroy(gameObject);
    }
}
