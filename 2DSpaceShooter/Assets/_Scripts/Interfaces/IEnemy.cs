using UnityEngine;
using UnityEngine.Splines;

public interface IEnemy
{
    public void SetEnemyData(EnemySO enemyData);
    public void SetGridPosition(Vector2 pos);
    public void SetSplineAnimateProperties(SplineContainer spline, float speed);
}
