using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalInputController : MonoBehaviour
{
    [SerializeField] private Vector2 _axisBoundsX;
    [SerializeField] private Vector2 _axisBoundsY;
    private int _normInputX, _normInputY;
    private Vector2Int _moveInput;
    [SerializeField] private Vector2 _moveSpeed = new(8f, 5f);
    [SerializeField] private Transform[] _weaponSpawnPoints;
    private bool _triggerAttack1 = false;
    private bool _triggerAttack2 = false;
    [SerializeField] private WeaponBase _currentPrimaryWeapon;
    [SerializeField] private WeaponBase _currentSecondaryWeapon;

    private void Start()
    {
        LoadWeapon(_currentPrimaryWeapon);
        LoadWeapon(_currentSecondaryWeapon);
    }

    private void LoadWeapon(WeaponBase weapon)
    {
        if (weapon != null)
            weapon.LoadWeapon(transform, _weaponSpawnPoints);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        _normInputX = (int)(context.ReadValue<Vector2>() * Vector2.right).normalized.x;
        _normInputY = (int)(context.ReadValue<Vector2>() * Vector2.up).normalized.y;
        _moveInput = new Vector2Int(_normInputX, _normInputY);
    }

    public void StartAttack1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _triggerAttack1 = true;
        }
        else if (context.canceled)
        {
            _triggerAttack1 = false;
        }
    }

    public void StartAttack2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _triggerAttack2 = true;
        }
        else if (context.canceled)
        {
            _triggerAttack2 = false;
        }
    }

    private void Update()
    {
        DoMovement();
        DoAttacks();
    }

    private void DoAttacks()
    {
        if (_currentPrimaryWeapon != null)
        {
            if (_currentPrimaryWeapon.IsWeaponAttacking())
            {
                _currentPrimaryWeapon.UpdateAttack();
            }
            else if ((_triggerAttack1 || _currentPrimaryWeapon.IsAutoAttack) && _currentPrimaryWeapon.CanAttackBeStarted())
            {
                _currentPrimaryWeapon.StartAttack();
            }
        }

        if (_currentSecondaryWeapon != null)
        {
            if (_currentSecondaryWeapon.IsWeaponAttacking())
            {
                _currentSecondaryWeapon.UpdateAttack();
            }
            else if ((_triggerAttack2 || _currentSecondaryWeapon.IsAutoAttack) && _currentSecondaryWeapon.CanAttackBeStarted())
            {
                _currentSecondaryWeapon.StartAttack();
            }
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
