using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable, IDestroyable
{
    EnemySO _enemyData;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        Destroy();
    }
}
