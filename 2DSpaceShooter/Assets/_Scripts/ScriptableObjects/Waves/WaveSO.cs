using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Create Enemy Wave")]
public class WaveSO : ScriptableObject
{
    public EnemySO Enemy;
    public int EnemyCount;
    public SPAWN_LOCATION_TYPE SpawnLocationType;
    public Vector2 SpawnPoint;
    public SPAWN_POSITION_TYPE SpawnPositionType;
    public Vector2 CustomSpawnPointMinimum;
    public Vector2 CustomSpawnPointMaximum;
    public float TimeBetweenEnemy;
    public float TimeBeforeWaveBegins;
    public float TimeBeforeWaveEnds;
}

public enum SPAWN_LOCATION_TYPE
{
    Point,
    Range
}

public enum SPAWN_POSITION_TYPE
{
    ScreenWidth,
    Custom
}
