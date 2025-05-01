using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBomb", menuName = "Weapons/Bomb")]
public class WeaponBomb : WeaponBase
{
    public float CooldownBetweenFiring = 2f;
    public float ProjectileSpeed = 2f;
    public bool IsPlayerProjectile;
    public GameObject DestructionParticles;
    public float DestroyWhenOffscreenDistance = 1f;
    public float MinimumTimeBeforeBombCanTrigger = 1f;
    public float ExplosionRadius;
    private Bomb _currentBomb;
    private float _earliestExplosionTime;
    private float _nextPossibleFireTime;

    public override void LoadWeapon(Transform thisTransform, Transform[] spawnPoints)
    {
        base.LoadWeapon(thisTransform, spawnPoints);

        _currentBomb = null;
        _timeWeaponLastFired = 0;
        _earliestExplosionTime = 0;
        _nextPossibleFireTime = 0;
    }

    public override bool CanAttackBeStarted()
    {
        return true;
    }

    public override bool IsWeaponAttacking()
    {
        return false;
    }

    public override void StartAttack()
    {
        if (_currentBomb == null && _nextPossibleFireTime < Time.time)
        {
            SpawnBomb();
            return;
        }

        if (_currentBomb != null && Time.time > _earliestExplosionTime)
        {
            _currentBomb.ExplodeBomb();
        }
    }

    public override void UpdateAttack()
    {
        Debug.LogError("This should never be called for the bomb");
    }

    public void MarkBombExploded()
    {
        _currentBomb = null;
        _timeWeaponLastFired = Time.time;
        _nextPossibleFireTime = Time.time + CooldownBetweenFiring;
    }

    private void SpawnBomb()
    {
        _earliestExplosionTime = Time.time + MinimumTimeBeforeBombCanTrigger;

        GameObject newBomb = ObjectPoolManager.SpawnObject(WeaponPrefab, _spawnPoints[0].position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.Projectile);
        _currentBomb = newBomb.GetComponent<Bomb>();
        _currentBomb.SetupBomb(this);
    }
}
