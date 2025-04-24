using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalInputController : MonoBehaviour
{
    [SerializeField] Vector2 _axisBoundsX;
    [SerializeField] Vector2 _axisBoundsY;
    int _normInputX, _normInputY;
    Vector2Int _moveInput;
    [SerializeField] Vector2 _moveSpeed = new(8f, 5f);
    [SerializeField] Transform[] _weaponSpawnPoints;
    bool _fireMissile = false;
    [SerializeField] WeaponBase _currentWeapon;

    private void Start()
    {
        LoadWeapon(_currentWeapon);
    }

    private void LoadWeapon(WeaponBase weapon)
    {
        weapon.LoadWeapon(transform, _weaponSpawnPoints);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        _normInputX = (int)(context.ReadValue<Vector2>() * Vector2.right).normalized.x;
        _normInputY = (int)(context.ReadValue<Vector2>() * Vector2.up).normalized.y;
        _moveInput = new Vector2Int(_normInputX, _normInputY);
    }

    public void Missile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _fireMissile = true;
        }
        else if (context.canceled)
        {
            _fireMissile = false;
        }
    }

    private void Update()
    {
        DoMovement();
        DoAttacks();
    }

    private void DoAttacks()
    {
        if (_currentWeapon.IsWeaponAttacking())
        {
            _currentWeapon.UpdateAttack();
        }
        else if ((_fireMissile || _currentWeapon.IsAutoAttack) && _currentWeapon.CanAttackBeStarted())
        {
            _currentWeapon.StartAttack();
        }
    }

    private void DoMovement()
    {
        Vector2 moveTo = _moveInput * _moveSpeed * Time.deltaTime;
        transform.Translate(moveTo.x, moveTo.y, 0f);

        // limit movement to screen size
        if (transform.position.y > GameManager.Instance.CameraOrthographicSize - _axisBoundsY.y)
        {
            transform.position = new Vector3(transform.position.x, GameManager.Instance.CameraOrthographicSize - _axisBoundsY.y, 0f);
        }
        else if (transform.position.y < -GameManager.Instance.CameraOrthographicSize + _axisBoundsY.x)
        {
            transform.position = new Vector3(transform.position.x, -GameManager.Instance.CameraOrthographicSize + _axisBoundsY.x, 0f);
        }
        if (transform.position.x > GameManager.Instance.AdjustedScreenWidth - _axisBoundsX.y)
        {
            transform.position = new Vector3(GameManager.Instance.AdjustedScreenWidth - _axisBoundsX.y, transform.position.y, 0f);
        }
        else if (transform.position.x < -GameManager.Instance.AdjustedScreenWidth + _axisBoundsX.x)
        {
            transform.position = new Vector3(-GameManager.Instance.AdjustedScreenWidth + _axisBoundsX.x, transform.position.y, 0f);
        }
    }
}
