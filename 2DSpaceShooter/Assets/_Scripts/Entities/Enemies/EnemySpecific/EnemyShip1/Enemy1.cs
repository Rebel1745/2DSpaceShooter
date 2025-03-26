using UnityEngine;
using UnityEngine.Splines;

public class Enemy1 : Entity, IDamageable, IDestroyable, IEnemy
{
    public void DestroyObject()
    {
        throw new System.NotImplementedException();
    }

    public void QuietDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void SetEnemyData(EnemySO enemyData)
    {
        throw new System.NotImplementedException();
    }

    public void SetGridPosition(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public void SetSplineAnimateProperties(SplineContainer spline, float speed)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }
}
