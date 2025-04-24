using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLaser", menuName = "Weapons/Laser")]
public class WeaponLaser : WeaponBase
{
    public float LaserLength;
    public float CooldownBetweenEnemyChecks = 0.1f;
    private float _timeOfLastEnemyCheck;
    private Collider2D[] _enemyColliders;
    private Transform _closestEnemy;
    private float _closestEnemyDistance;
    private LineRenderer _laserLine;
    public GameObject LaserLineRendererPrefab;
    private Transform _laserSpawnPoint;

    public override void LoadWeapon(Transform thisTransform, Transform[] spawnPoints)
    {
        base.LoadWeapon(thisTransform, spawnPoints);

        // check to see if we already have created a line renderer for our laser
        _laserLine = thisTransform.GetComponentInChildren<LineRenderer>();
        _laserSpawnPoint = _spawnPoints[_spawnPointDetails[0].SpawnPointIndex];

        // if not, create one
        if (!_laserLine)
        {
            Quaternion angle = Quaternion.Euler(0f, 0f, _spawnPointDetails[0].SpawnPointAngle);
            GameObject laserLineRenderer = Instantiate(LaserLineRendererPrefab, _laserSpawnPoint.position, angle, thisTransform);
            _laserLine = laserLineRenderer.GetComponent<LineRenderer>();
        }

        // clear all previous points on the laser line
        _laserLine.positionCount = 0;

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
        // check to see if the cooldown period has passed for checking for enemies, otherwise bail
        if (_timeOfLastEnemyCheck + CooldownBetweenEnemyChecks < Time.time) return;

        // find all enemies around the player within the given laser length
        _enemyColliders = Physics2D.OverlapCircleAll(_laserSpawnPoint.position, LaserLength, WhatIsEnemy);
        _timeOfLastEnemyCheck = Time.time;
        _closestEnemy = null;
        _closestEnemyDistance = Mathf.Infinity;

        // if there are no enemies, bail out here
        if (_enemyColliders.Length == 0)
        {
            _laserLine.positionCount = 0;
            return;
        }

        // go through the enemies and find the closest
        foreach (Collider2D col in _enemyColliders)
        {
            float dist = Vector3.Distance(_laserSpawnPoint.position, col.transform.position);
            if (dist < _closestEnemyDistance)
            {
                _closestEnemy = col.transform;
                _closestEnemyDistance = dist;
            }
        }

        Debug.Log(_laserSpawnPoint.position + " - " + _closestEnemy.position);
        // update the line renderer (maybe use something else in the future) to go from the weapon spawn point to the enemy
        Vector3[] positions = { _laserSpawnPoint.position, _closestEnemy.position };
        _laserLine.positionCount = 2;
        _laserLine.SetPositions(positions);
    }
}
