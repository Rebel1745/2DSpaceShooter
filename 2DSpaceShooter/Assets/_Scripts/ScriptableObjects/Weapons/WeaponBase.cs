using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public GameObject WeaponPrefab;
    public bool IsAutoAttack = false;
    public PlayerWeaponSpawnPoint[] _spawnPointDetails;
    public LayerMask WhatIsEnemy;
    protected Transform[] _spawnPoints;
    protected float _timeWeaponLastFired = 0f;
    protected bool _isWeaponAttacking = false;

    public virtual void LoadWeapon(Transform thisTransform, Transform[] spawnPoints)
    {
        _spawnPoints = spawnPoints;
        _timeWeaponLastFired = 0;
        _isWeaponAttacking = false;
    }

    public abstract bool CanAttackBeStarted();
    public abstract bool IsWeaponAttacking();
    public abstract void StartAttack();
    public abstract void UpdateAttack();
}
