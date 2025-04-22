using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Weapons/Projectile")]
public class WeaponProjectile : WeaponBase
{
    public float ProjectileSpeed = 5f;
    public float TimeUntilProjectileDestroyed = 2f;
    public float Cooldown = 0.5f;
    public bool AutoFire = false;
    public int Damage = 1;
    public bool IsPlayerProjectile;
    public GameObject DestructionParticles;
    public float DestroyWhenOffscreenDistance = 0f;

    public override bool CanAttackBeStarted()
    {
        if (_isWeaponAttacking) return false;
        if (_timeWeaponLastFired + Cooldown > Time.time) return false;

        return true;
    }

    public override bool IsWeaponAttacking()
    {
        return _isWeaponAttacking;
    }

    public override void StartAttack()
    {
        _isWeaponAttacking = true;
        _timeWeaponLastFired = Time.time;

        InitiateProjectile(WeaponPrefab, _spawnPoints[0].position, _spawnPoints[0].rotation);
        _isWeaponAttacking = false;
    }

    public override void UpdateAttack()
    {

    }

    private void InitiateProjectile(GameObject prefab, Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject newMissile = ObjectPoolManager.SpawnObject(prefab, spawnPoint, rotation, ObjectPoolManager.POOL_TYPE.Projectile);
        newMissile.GetComponent<Missile>().SetupMissile(this);
    }
}
