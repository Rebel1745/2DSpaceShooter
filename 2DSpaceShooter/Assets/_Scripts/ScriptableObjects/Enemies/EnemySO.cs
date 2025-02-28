using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject EnemyPrefab;
    public GameObject DestructionParticles;
    public float EnemySpeed = 2f;
}
