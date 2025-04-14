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

    public void TakeDamage(int amount)
    {
        _currentHealth -= 1;

        if (_currentHealth <= 0) DestroyObject();
    }
}
