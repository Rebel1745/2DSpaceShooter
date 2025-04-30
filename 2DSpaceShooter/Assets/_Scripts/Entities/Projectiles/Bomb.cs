using UnityEngine;
using UnityEngine.Events;

public class Bomb : Entity, IDestroyable
{
    private WeaponBomb _bombData;
    private float _bombSpawnTime;

    public void SetupBomb(WeaponBomb bombData)
    {
        _bombData = bombData;
        GetComponent<MoveForward>().SetSpeed(_bombData.ProjectileSpeed);
        gameObject.layer = _bombData.IsPlayerProjectile ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("EnemyProjectile");
        _bombSpawnTime = Time.time;
    }

    public bool ExplodeBomb(bool loud)
    {
        // if the bomb has not been alive long enough to trigger, bail
        if (_bombSpawnTime + _bombData.MinimumTimeBeforeBombCanTrigger < Time.time) return false;

        // time to explode
        if (loud)
            DestroyObject();
        else
            QuietDestroy();

        return true;
    }

    public void DestroyObject()
    {
        if (_bombData.DestructionParticles)
            ObjectPoolManager.SpawnObject(_bombData.DestructionParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void QuietDestroy()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
