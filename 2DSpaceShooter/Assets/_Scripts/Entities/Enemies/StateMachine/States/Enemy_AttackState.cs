using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData) : base(enemy, stateMachine, enemyData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameObject newMissile = ObjectPoolManager.SpawnObject(_enemyData.Projectile.ProjectilePrefab, _enemy.AttackSpawnPoint.position, _enemy.AttackSpawnPoint.rotation, ObjectPoolManager.POOL_TYPE.Projectile);
        newMissile.GetComponent<Missile>().SetupMissile(_enemyData.Projectile);
        _enemy.SetNextAttackTime();
        _stateMachine.ChangeState(_stateMachine.PreviousState);
    }
}
