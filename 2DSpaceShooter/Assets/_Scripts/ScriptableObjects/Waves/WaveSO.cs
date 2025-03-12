using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Create Enemy Wave")]
public class WaveSO : ScriptableObject
{
    public EnemySO[] Enemies;
    public int EnemyCount;
    public int EnemyRows;
    public bool RandomEnemy = false;
    public SPAWN_LOCATION_TYPE SpawnLocationType;
    public float SpawnPointX;
    public Vector2 SpawnPointXRange;
    public float TimeBetweenEnemy;
    public float TimeBeforeWaveBegins;
    public float TimeBeforeWaveEnds;
    public bool WaitForEnemiesToBeDestroyedBeforeNextWave;
}

public enum SPAWN_LOCATION_TYPE
{
    Point,
    Range
}
