using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Weapons/Projectile")]
public class WeaponProjectile : WeaponBase
{
    public int NumberOfShotsInBurst = 1;
    public float TimeBetweenBurstShots = 0.25f;
    public float CooldownBetweenBursts = 0.5f;
    public float ProjectileSpeed = 5f;
    public float TimeUntilProjectileDestroyed = 2f;
    public int Damage = 1;
    public bool IsPlayerProjectile;
    public GameObject DestructionParticles;
    public float DestroyWhenOffscreenDistance = 0f;
    private int _currentShotsFired = 0;

    public override bool CanAttackBeStarted()
    {
        if (_isWeaponAttacking) return false;
        if (_timeWeaponLastFired + CooldownBetweenBursts > Time.time) return false;

        return true;
    }

    public override bool IsWeaponAttacking()
    {
        return _isWeaponAttacking;
    }

    public override void StartAttack()
    {
        _isWeaponAttacking = true;
        _currentShotsFired = 0;
        FireFromAllSpawnPoints();
    }

    public override void UpdateAttack()
    {
        // if we have completed the number of shots, we are no longer attacking and can bail
        if (_currentShotsFired == NumberOfShotsInBurst)
        {
            _isWeaponAttacking = false;
            return;
        }

        // if we have not yet reached the time between burst shots, bail
        if (_timeWeaponLastFired + TimeBetweenBurstShots > Time.time) return;

        // if we get here, then we can fire the next shot of the burst
        FireFromAllSpawnPoints();
    }

    private void FireFromAllSpawnPoints()
    {
        foreach (PlayerWeaponSpawnPoint pwsp in _spawnPointDetails)
        {
            // first, check if the spawn point exisits
            if (pwsp.SpawnPointIndex >= _spawnPoints.Length)
            {
                Debug.LogError("SpawnPoint does not exist. Sort this out");
                return;
            }
            Quaternion angle = Quaternion.Euler(0f, 0f, pwsp.SpawnPointAngle);
            InitiateProjectile(WeaponPrefab, _spawnPoints[pwsp.SpawnPointIndex].position, angle);
        }
        _currentShotsFired++;
        _timeWeaponLastFired = Time.time;
    }

    private void InitiateProjectile(GameObject prefab, Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject newMissile = ObjectPoolManager.SpawnObject(prefab, spawnPoint, rotation, ObjectPoolManager.POOL_TYPE.Projectile);
        newMissile.GetComponent<Missile>().SetupMissile(this);
    }
}
