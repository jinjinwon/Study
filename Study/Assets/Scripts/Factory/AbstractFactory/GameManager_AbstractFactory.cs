using UnityEngine;

public class GameManager_AbstractFactory : MonoBehaviour
{
    void Start()
    {
        // Slime Factory 사용
        IEnemyFactory slimeFactory = new SlimeFactory_Abstract();
        IEnemy slime = slimeFactory.CreateEnemy();
        IWeapon slimeWeapon = slimeFactory.CreateWeapon();

        slime.Attack();          
        slimeWeapon.Use();       

        // Goblin Factory 사용
        IEnemyFactory goblinFactory = new GoblinFactory_Abstract();
        IEnemy goblin = goblinFactory.CreateEnemy();
        IWeapon goblinWeapon = goblinFactory.CreateWeapon();

        goblin.Attack();         
        goblinWeapon.Use();    

        // Goblin Factory 사용
        IEnemyFactory dragonFactory = new DragonFactory_Abstract();
        IEnemy dragon = dragonFactory.CreateEnemy();
        IWeapon dragonWeapon = dragonFactory.CreateWeapon();

        dragon.Attack();
        dragonWeapon.Use();
    }
}
