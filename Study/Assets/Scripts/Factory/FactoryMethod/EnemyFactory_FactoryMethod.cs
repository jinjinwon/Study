using UnityEngine;

public abstract class EnemyFactory
{
    // Factory Method
    public abstract IEnemy_FactoryMethod CreateEnemy();

    // 공통 로직
    public void SpawnEnemy()
    {
        IEnemy_FactoryMethod enemy = CreateEnemy();
        enemy.Attack();
    }
}
