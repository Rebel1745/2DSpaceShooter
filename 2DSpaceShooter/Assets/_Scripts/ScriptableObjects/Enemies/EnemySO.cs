using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject EnemyPrefab;
    public GameObject DestructionParticles;
    public float EnemySpeed = 2f;
    public float DestroyWhenOffscreenDistance = 0f;
    public bool RotateSpriteRandomly = false;
}
