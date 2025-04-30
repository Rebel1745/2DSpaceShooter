using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBomb", menuName = "Weapons/Bomb")]
public class WeaponBomb : WeaponBase
{
    public float CooldownBetweenFiring = 2f;
    public int MaximumBombsOnscreen = 1;
    public float ProjectileSpeed = 2f;
    public bool IsPlayerProjectile;
    public GameObject DestructionParticles;
    public float DestroyWhenOffscreenDistance = 1f;
    public float MinimumTimeBeforeBombCanTrigger = 1f;
    public float ExplosionRadius;
    private Dictionary<Bomb, float> _currentBombs;

    public override void LoadWeapon(Transform thisTransform, Transform[] spawnPoints)
    {
        base.LoadWeapon(thisTransform, spawnPoints);

        _currentBombs = new Dictionary<Bomb, float>();
    }

    public override bool CanAttackBeStarted()
    {
        if (_currentBombs.Count == MaximumBombsOnscreen) return false;
        if (_timeWeaponLastFired + CooldownBetweenFiring < Time.time) return false;

        return true;
    }

    public override bool IsWeaponAttacking()
    {
        return _isWeaponAttacking;
    }

    public override void StartAttack()
    {
        /*if (_currentBomb == null)
            SpawnBomb();
        else
            ExplodeBomb();*/
    }

    public override void UpdateAttack()
    {
        throw new NotImplementedException();
    }

    private void ExplodeBomb()
    {
        /*if (_currentBomb.ExplodeBomb(true))
        {
            _currentBomb = null;
        }*/
    }

    private void SpawnBomb()
    {
        /*GameObject newBomb = ObjectPoolManager.SpawnObject(WeaponPrefab, _spawnPoints[0].position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.Projectile);
        _currentBomb = newBomb.GetComponent<Bomb>();
        _currentBomb.SetupBomb(this);*/
    }
}
