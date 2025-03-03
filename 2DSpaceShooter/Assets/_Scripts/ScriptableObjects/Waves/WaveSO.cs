using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Create Enemy Wave")]
public class WaveSO : ScriptableObject
{
    public EnemySO Enemy;
    public int EnemyCount;
    public SPAWN_LOCATION_TYPE SpawnLocationType;
    public float SpawnPointX;
    public float TimeBetweenEnemy;
    public float TimeBeforeWaveBegins;
    public float TimeBeforeWaveEnds;
}

public enum SPAWN_LOCATION_TYPE
{
    Point,
    Range
}
