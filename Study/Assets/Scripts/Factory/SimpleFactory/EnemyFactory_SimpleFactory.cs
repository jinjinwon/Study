using UnityEngine;

public class EnemyFactory_SimpleFactory
{
    public static IEnemy_SimpleFactory CreateEnemy(string enemyType)
    {
        switch (enemyType)
        {
            case "Slime":
                return new Slime();
            case "Goblin":
                return new Goblin();
            case "Dragon":
                return new Dragon();
            default:
                throw new System.ArgumentException("Invalid enemy type");
        }
    }
}
