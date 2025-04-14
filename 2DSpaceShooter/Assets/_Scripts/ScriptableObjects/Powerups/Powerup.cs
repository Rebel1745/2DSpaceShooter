using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupEffectBase _powerup;

    void OnTriggerEnter2D(Collider2D collision)
    {
        _powerup.ApplyEffect(collision.gameObject);
        // maybe make this a pooled object?
        Destroy(gameObject);
    }
}
