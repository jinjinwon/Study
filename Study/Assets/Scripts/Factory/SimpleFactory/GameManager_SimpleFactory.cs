using UnityEngine;

public class GameManager_SimpleFactory : MonoBehaviour
{
    void Start()
    {
        // Àû »ý¼º
        IEnemy_SimpleFactory slime = EnemyFactory_SimpleFactory.CreateEnemy("Slime");
        slime.Attack();

        IEnemy_SimpleFactory goblin = EnemyFactory_SimpleFactory.CreateEnemy("Goblin");
        goblin.Attack();

        IEnemy_SimpleFactory dragon = EnemyFactory_SimpleFactory.CreateEnemy("Dragon");
        dragon.Attack(); 
    }
}
