using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Projectile")]
public class ProjectileSO : ScriptableObject
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 5f;
    public float TimeUntilProjectileDestroyed = 2f;
    public float Cooldown = 0.5f;
    public bool AutoFire = false;
    public int Damage = 1;
    public LayerMask ProjectileLayer;
    public GameObject DestructionParticles;
    public float DestroyWhenOffscreenDistance = 0f;
}
