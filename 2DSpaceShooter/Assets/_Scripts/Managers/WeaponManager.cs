using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }
    [SerializeField] private WeaponBase[] _primaryWeapons;
    public WeaponBase CurrentPrimaryWeapon { get; private set; }
    [SerializeField] private WeaponBase[] _secondaryWeapons;
    public WeaponBase CurrentSecondaryWeapon { get; private set; }

    public UnityEvent<WeaponBase> OnPrimaryWeaponChanged;
    public UnityEvent<WeaponBase> OnsecondaryWeaponChanged;

    private bool _secondaryWeaponSwitch = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (CurrentPrimaryWeapon == null) SwitchPrimaryWeapon(0);

        if (CurrentSecondaryWeapon == null) SwitchSecondaryWeapon(0);
    }

    public void SwitchWeapon(int weaponNumber)
    {
        if (_secondaryWeaponSwitch) SwitchSecondaryWeapon(weaponNumber);
        else SwitchPrimaryWeapon(weaponNumber);
    }

    private void SwitchPrimaryWeapon(int weaponNumber)
    {
        if (_primaryWeapons[weaponNumber] != null)
        {
            CurrentPrimaryWeapon = _primaryWeapons[weaponNumber];
            OnPrimaryWeaponChanged?.Invoke(CurrentPrimaryWeapon);
        }
    }

    private void SwitchSecondaryWeapon(int weaponNumber)
    {
        if (_secondaryWeapons[weaponNumber] != null)
        {
            CurrentSecondaryWeapon = _secondaryWeapons[weaponNumber];
            OnsecondaryWeaponChanged?.Invoke(CurrentSecondaryWeapon);
        }
    }

    public void SetWeaponTypeToSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
            _secondaryWeaponSwitch = true;

        if (context.canceled)
            _secondaryWeaponSwitch = false;
    }
}
