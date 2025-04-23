using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    public GameObject WeaponPrefab;
    public bool IsAutoAttack = false;
    public PlayerWeaponSpawnPoint[] _spawnPointDetails;
    protected Transform[] _spawnPoints;
    protected float _timeWeaponLastFired = 0f;
    protected bool _isWeaponAttacking = false;

    public void LoadWeapon(Transform[] spawnPoints)
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
