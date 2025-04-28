using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLaser", menuName = "Weapons/Laser")]
public class WeaponLaser : WeaponBase
{
    [Range(1, 5)] public int MaximumNumberOfLasers;
    public float LaserLength;
    public float CooldownBetweenEnemyChecks = 0.1f;
    private float _timeOfNextEnemyCheck;
    private Collider2D[] _enemyColliders;
    private Collider2D _closestEnemy;
    private float _closestEnemyDistance;
    private LineRenderer[] _laserLines;
    public GameObject LaserLineRendererPrefab;
    private Transform _laserSpawnPoint;
    private Dictionary<Collider2D, float> _enemyDistances;
    private float _damagePerTick;

    public override void LoadWeapon(Transform thisTransform, Transform[] spawnPoints)
    {
        base.LoadWeapon(thisTransform, spawnPoints);

        _laserSpawnPoint = _spawnPoints[_spawnPointDetails[0].SpawnPointIndex];
        _enemyColliders = new Collider2D[0];

        // remove all previous line renderers
        _laserLines = thisTransform.GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer lr in _laserLines)
        {
            Destroy(lr.gameObject);
        }

        // create all new line renderers
        _laserLines = new LineRenderer[MaximumNumberOfLasers];

        for (int i = 0; i < MaximumNumberOfLasers; i++)
        {
            GameObject laserLineRenderer = Instantiate(LaserLineRendererPrefab, _laserSpawnPoint.position, quaternion.identity, thisTransform);
            _laserLines[i] = laserLineRenderer.GetComponent<LineRenderer>();
        }

        ClearLaserLines();

        _timeOfNextEnemyCheck = CooldownBetweenEnemyChecks;
        _damagePerTick = Damage / (1 / CooldownBetweenEnemyChecks);

        // the laser HAS to be auto attack
        IsAutoAttack = true;
    }

    public override bool CanAttackBeStarted()
    {
        if (_isWeaponAttacking) return false;

        return true;
    }

    public override bool IsWeaponAttacking()
    {
        return _isWeaponAttacking;
    }

    public override void StartAttack()
    {
        if (_spawnPointDetails.Length > 1)
        {
            Debug.LogError("Lasers can only come from one point. Sort this!!");
            return;
        }

        _isWeaponAttacking = true;

        // just call update attack right away as that is where the meat of the laser fun happens
        UpdateAttack();
    }

    public override void UpdateAttack()
    {
        // the lasers should always be updated to keep a smoothly moving line between player and target(s)
        UpdateLaserLines();

        // check to see if the cooldown period has passed for checking for enemies, otherwise bail
        if (Time.time < _timeOfNextEnemyCheck) return;

        // find all enemies around the player within the given laser length
        _enemyColliders = Physics2D.OverlapCircleAll(_laserSpawnPoint.position, LaserLength, WhatIsEnemy);
        _timeOfNextEnemyCheck = Time.time + CooldownBetweenEnemyChecks;

        // if there are no enemies, bail out here
        if (_enemyColliders.Length == 0)
        {
            ClearLaserLines();
            return;
        }

        if (MaximumNumberOfLasers == 1)
            SingleLaser();
        else
            MultipleLasers();
    }

    private void MultipleLasers()
    {
        // make a dictionary of all of the enemies and their distances
        _enemyDistances = new Dictionary<Collider2D, float>();

        foreach (Collider2D col in _enemyColliders)
        {
            _enemyDistances.Add(col, Vector3.Distance(_laserSpawnPoint.position, col.transform.position));
        }

        // sort the dictionary by the distance if we have more enemies than lasers
        if (_enemyColliders.Length > MaximumNumberOfLasers)
        {
            _enemyDistances = _enemyDistances.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        // turn the dictionary into an array of enemies
        _enemyColliders = _enemyDistances.Keys.ToArray();

        // do damage to all the selected enemies
        for (int i = 0; i < _enemyColliders.Length; i++)
        {
            if (i < _laserLines.Length)
            {
                DoDamage(_enemyColliders[i].gameObject);
            }
        }
    }

    private void SingleLaser()
    {
        _closestEnemy = null;
        _closestEnemyDistance = Mathf.Infinity;

        // go through the enemies and find the closest
        foreach (Collider2D col in _enemyColliders)
        {
            float dist = Vector3.Distance(_laserSpawnPoint.position, col.transform.position);
            if (dist < _closestEnemyDistance)
            {
                _closestEnemy = col;
                _closestEnemyDistance = dist;
            }
        }

        _enemyColliders = new Collider2D[1];
        _enemyColliders[0] = _closestEnemy;

        // do damage to the targeted enemy
        DoDamage(_enemyColliders[0].gameObject);
    }

    private void ClearLaserLines()
    {
        foreach (LineRenderer lr in _laserLines)
        {
            // clear all previous points on the laser line
            lr.positionCount = 0;
        }
    }

    private void ClearLaserLine(int lineIndex)
    {
        _laserLines[lineIndex].positionCount = 0;
    }

    private void UpdateLaserLines()
    {
        for (int i = 0; i < _enemyColliders.Length; i++)
        {
            if (i < _laserLines.Length)
            {
                Vector3[] positions = { _laserSpawnPoint.position, _enemyColliders[i].transform.position };
                _laserLines[i].positionCount = 2;
                _laserLines[i].SetPositions(positions);
                //DoDamage(_enemyColliders[i].gameObject);
            }
        }

        if (_enemyColliders.Length < _laserLines.Length)
        {
            for (int i = _enemyColliders.Length; i < _laserLines.Length; i++)
            {
                ClearLaserLine(i);
            }
        }
    }

    private void DoDamage(GameObject target)
    {
        IDamageable id = target.GetComponent<IDamageable>();

        // if we are going to destroy an enemy, trigger an imidiate recheck of near enemies
        if (id.GetCurrentHealth() - _damagePerTick <= 0f)
            _timeOfNextEnemyCheck = 0f;

        id.TakeDamage(_damagePerTick);
    }
}
