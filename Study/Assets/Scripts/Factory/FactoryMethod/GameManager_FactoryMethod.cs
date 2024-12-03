using UnityEngine;

public class GameManager_FactoryMethod : MonoBehaviour
{
    void Start()
    {
        EnemyFactory slimeFactory = new SlimeFactory();
        slimeFactory.SpawnEnemy();

        EnemyFactory goblinFactory = new GoblinFactory();
        goblinFactory.SpawnEnemy();

        EnemyFactory dragonFactory = new DragonFactory();
        dragonFactory.SpawnEnemy(); 
    }
}

