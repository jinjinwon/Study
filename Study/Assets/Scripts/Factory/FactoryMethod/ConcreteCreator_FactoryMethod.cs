using UnityEngine;

public class SlimeFactory : EnemyFactory
{
    public override IEnemy_FactoryMethod CreateEnemy()
    {
        return new Slime_FactoryMethod();
    }
}

public class GoblinFactory : EnemyFactory
{
    public override IEnemy_FactoryMethod CreateEnemy()
    {
        return new Goblin_FactoryMethod();
    }
}

public class DragonFactory : EnemyFactory
{
    public override IEnemy_FactoryMethod CreateEnemy()
    {
        return new Dragon_FactoryMethod();
    }
}