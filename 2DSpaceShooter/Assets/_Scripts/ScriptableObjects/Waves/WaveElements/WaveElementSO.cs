using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveElement", menuName = "Create Enemy Wave Element")]
public class WaveElementSO : ScriptableObject
{
    public EnemySO[] Enemies;
    public int EnemyCount;
    public bool RandomEnemy = false;
    public SPAWN_LOCATION_TYPE SpawnLocationType;
    public float SpawnPointX;
    public Vector2 SpawnPointXRange;
    public float TimeBetweenEnemy;
    public float TimeBeforeWaveBegins;
    public float TimeBeforeWaveEnds;
    public bool WaitForEnemiesToBeDestroyedBeforeNextWaveElement = false;
}

public enum SPAWN_LOCATION_TYPE
{
    Point,
    Range
}
