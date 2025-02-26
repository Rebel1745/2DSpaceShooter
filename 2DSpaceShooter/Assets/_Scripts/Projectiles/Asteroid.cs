using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable, IDestroyable
{
    [SerializeField] EnemySO _enemyData;

    public void DestroyObject()
    {
        if (_enemyData.DestructionParticles)
            Instantiate(_enemyData.DestructionParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        DestroyObject();
    }
}
