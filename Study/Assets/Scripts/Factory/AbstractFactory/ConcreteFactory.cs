using UnityEngine;

public class SlimeFactory_Abstract : IEnemyFactory
{
    public IEnemy CreateEnemy()
    {
        return new Slime_Abstract();
    }

    public IWeapon CreateWeapon()
    {
        return new SlimeWeapon();
    }
}

public class GoblinFactory_Abstract : IEnemyFactory
{
    public IEnemy CreateEnemy()
    {
        return new Goblin_Abstract();
    }

    public IWeapon CreateWeapon()
    {
        return new GoblinWeapon();
    }
}

public class DragonFactory_Abstract : IEnemyFactory
{
    public IEnemy CreateEnemy()
    {
        return new Dragon_Abstract();
    }

    public IWeapon CreateWeapon()
    {
        return new DragonWeapon();
    }
}