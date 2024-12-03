using UnityEngine;

public class Slime_Abstract : IEnemy
{
    public void Attack()
    {
        Debug.Log("Slime attacks by jumping!");
    }
}

public class SlimeWeapon : IWeapon
{
    public void Use()
    {
        Debug.Log("Slime throws a sticky ball!");
    }
}

public class Goblin_Abstract : IEnemy
{
    public void Attack()
    {
        Debug.Log("Goblin attacks with a club!");
    }
}

public class GoblinWeapon : IWeapon
{
    public void Use()
    {
        Debug.Log("Goblin swings its club!");
    }
}

public class Dragon_Abstract : IEnemy
{
    public void Attack()
    {
        Debug.Log("Dragon attacks with fire breath!");
    }
}

public class DragonWeapon : IWeapon
{
    public void Use()
    {
        Debug.Log("Dragon breathes fire through its weapon!");
    }
}
