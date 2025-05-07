using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalInputController : MonoBehaviour
{
    [SerializeField] PlayerInput pI;
    [SerializeField] private Vector2 _axisBoundsX;
    [SerializeField] private Vector2 _axisBoundsY;
    private float _inputX, _inputY;
    private int _normInputX, _normInputY;
    private Vector2 _moveInput;
    [SerializeField] private Vector2 _moveSpeed = new(8f, 5f);
    [SerializeField] private Transform[] _weaponSpawnPoints;
    private bool _triggerAttack1 = false;
    private bool _triggerAttack2 = false;
    private WeaponBase _currentPrimaryWeapon;
    private WeaponBase _currentSecondaryWeapon;

    private void Start()
    {
        SetPrimaryWeapon(WeaponManager.Instance.CurrentPrimaryWeapon);
        SetSecondaryWeapon(WeaponManager.Instance.CurrentSecondaryWeapon);

        WeaponManager.Instance.OnPrimaryWeaponChanged.AddListener(SetPrimaryWeapon);
        WeaponManager.Instance.OnsecondaryWeaponChanged.AddListener(SetSecondaryWeapon);
    }

    void OnEnable()
    {
        if (WeaponManager.Instance)
        {
            WeaponManager.Instance.OnPrimaryWeaponChanged.RemoveListener(SetPrimaryWeapon);
            WeaponManager.Instance.OnsecondaryWeaponChanged.RemoveListener(SetSecondaryWeapon);

            WeaponManager.Instance.OnPrimaryWeaponChanged.AddListener(SetPrimaryWeapon);
            WeaponManager.Instance.OnsecondaryWeaponChanged.AddListener(SetSecondaryWeapon);
        }
    }

    void OnDisable()
    {
        WeaponManager.Instance.OnPrimaryWeaponChanged.RemoveListener(SetPrimaryWeapon);
        WeaponManager.Instance.OnsecondaryWeaponChanged.RemoveListener(SetSecondaryWeapon);
    }

    private void SetPrimaryWeapon(WeaponBase weapon)
    {
        _currentPrimaryWeapon = weapon;
        _currentPrimaryWeapon.LoadWeapon(transform, _weaponSpawnPoints);
    }

    private void SetSecondaryWeapon(WeaponBase weapon)
    {
        _currentSecondaryWeapon = weapon;
        _currentSecondaryWeapon.LoadWeapon(transform, _weaponSpawnPoints);
    }

    private void LoadWeapon(WeaponBase weapon)
    {
        if (weapon != null)
            weapon.LoadWeapon(transform, _weaponSpawnPoints);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        _inputX = (context.ReadValue<Vector2>() * Vector2.right).x;
        _inputY = (context.ReadValue<Vector2>() * Vector2.up).y;
        _normInputX = (int)(context.ReadValue<Vector2>() * Vector2.right).normalized.x;
        _normInputY = (int)(context.ReadValue<Vector2>() * Vector2.up).normalized.y;

        if (pI.currentControlScheme == "Gamepad")
            _moveInput = new Vector2(_inputX, _inputY);
        else
            _moveInput = new Vector2(_normInputX, _normInputY);
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
