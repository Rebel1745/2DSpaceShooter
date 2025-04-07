using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Create Enemy Wave")]
public class WaveSO : ScriptableObject
{
    public WaveElementSO[] WaveElements;
    public int EnemyRows;
    public float TimeBeforeWaveBegins;
    public float TimeBeforeWaveEnds;
    public bool WaitForEnemiesToBeDestroyedBeforeNextWave;
}
