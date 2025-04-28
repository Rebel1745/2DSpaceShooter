using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IDestroyable
{
    [SerializeField] GameObject _deathParticles;
    [SerializeField] float _startingHealth;
    float _currentHealth;
    public float CurrentHealth { get { return _currentHealth; } }

    void Start()
    {
        _currentHealth = _startingHealth;
    }

    public void DestroyObject()
    {
        if (_deathParticles)
        {
            //Instantiate(_deathParticles, transform.position, Quaternion.identity);
            ObjectPoolManager.SpawnObject(_deathParticles, transform.position, Quaternion.identity, ObjectPoolManager.POOL_TYPE.ParticleSystem);
        }

        Destroy(gameObject);
    }

    public void QuietDestroy()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0) DestroyObject();

    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
}
