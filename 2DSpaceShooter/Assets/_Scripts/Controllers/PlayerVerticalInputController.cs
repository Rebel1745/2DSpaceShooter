using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalInputController : MonoBehaviour
{
    Vector2 _moveInput;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] Transform _missileSpawnPoint;
    [SerializeField] Transform _missleHolder;
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] float _missileSpeed = 5f;
    [SerializeField] float _timeUntilMissileDestroyed = 2f;

    public void Movement(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>().normalized;
    }

    public void Missile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FireMissile();
        }
    }

    private void FireMissile()
    {
        GameObject newMissile = Instantiate(_missilePrefab, _missileSpawnPoint.position, _missileSpawnPoint.rotation, _missleHolder);
        newMissile.GetComponent<Missile>().SetMissileSpeed(_missileSpeed);
        newMissile.GetComponent<DestroyAfterTime>().SetDestroyAfterTime(_timeUntilMissileDestroyed);
    }

    private void Update()
    {
        Vector2 moveTo = _moveInput * _moveSpeed * Time.deltaTime;
        transform.Translate(moveTo.x, moveTo.y, 0f);
    }
}
