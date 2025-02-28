using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Create Level")]
public class LevelSO : ScriptableObject
{
    public WaveSO[] Waves;
}
