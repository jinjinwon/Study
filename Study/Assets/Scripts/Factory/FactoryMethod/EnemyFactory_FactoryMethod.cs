using UnityEngine;

public abstract class EnemyFactory
{
    // Factory Method
    public abstract IEnemy_FactoryMethod CreateEnemy();

    // ���� ����
    public void SpawnEnemy()
    {
        IEnemy_FactoryMethod enemy = CreateEnemy();
        enemy.Attack();
    }
}
