using UnityEngine;

public class E_EnemyShip1_AttackState : Enemy_AttackState
{
    private E_EnemyShip1 _enemyShip1;

    public E_EnemyShip1_AttackState(Enemy enemy, EnemyStateMachine stateMachine, EnemySO enemyData, E_EnemyShip1 enemyShip1) : base(enemy, stateMachine, enemyData)
    {
        _enemyShip1 = enemyShip1;
    }

    public override void Enter()
    {
        base.Enter();

        // TODO: Sort enemy attack
        GameObject newMissile = ObjectPoolManager.SpawnObject(_enemyShip1._projectile.WeaponPrefab, _enemy.AttackSpawnPoint.position, _enemy.AttackSpawnPoint.rotation, ObjectPoolManager.POOL_TYPE.Projectile);
        newMissile.GetComponent<Missile>().SetupMissile(_enemyShip1._projectile);
        _enemy.SetNextAttackTime();
        _stateMachine.ChangeState(_stateMachine.PreviousState);
    }
}
