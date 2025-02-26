using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IDestroyable
{
    [SerializeField] GameObject _deathParticles;
    [SerializeField] int _startingHealth;
    int _currentHealth;
    public float CurrentHealth { get { return _currentHealth; } }

    void Start()
    {
        _currentHealth = _startingHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(1);
    }

    public void DestroyObject()
    {
        if (_deathParticles)
        {
            Instantiate(_deathParticles, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= 1;

        if (_currentHealth <= 0) DestroyObject();
    }
}
