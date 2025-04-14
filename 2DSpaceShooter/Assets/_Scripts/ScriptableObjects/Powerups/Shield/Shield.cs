using UnityEngine;

public class Shield : MonoBehaviour, IDamageable, IDestroyable
{
    [SerializeField] private GameObject _shieldGO;
    [SerializeField] private Collider2D _objectCollider;
    [SerializeField] private Collider2D _shieldCollider;
    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private Color _healthyShieldColour = Color.green;
    [SerializeField] private Color _unhealthyShieldColour = Color.red;
    private int _currentHitAmount;

    public void ApplyShield(int hitAmount)
    {
        _currentHitAmount = hitAmount;

        if (hitAmount == 1) _shieldSprite.color = _unhealthyShieldColour;
        else _shieldSprite.color = _healthyShieldColour;

        _shieldGO.SetActive(true);
        _objectCollider.enabled = false;
        _shieldCollider.enabled = true;
    }

    public void DestroyObject()
    {
        _objectCollider.enabled = true;
        _shieldCollider.enabled = false;
        _shieldGO.SetActive(false);
    }

    public void QuietDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount)
    {
        _currentHitAmount--;

        if (_currentHitAmount == 1) _shieldSprite.color = _unhealthyShieldColour;

        else if (_currentHitAmount <= 0) DestroyObject();
    }
}
