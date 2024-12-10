using UnityEngine;
using System.Collections.Generic;

public class CharacterTest : MonoBehaviour
{
    public List<Stat> BaseStats;     
    public ItemData Sword;            
    public ItemData Shield;           

    [HideInInspector] public CharacterStats _characterStats;

    private void Awake()
    {
        _characterStats = new CharacterStats(BaseStats);

        PrintStats("ÃÊ±â ½ºÅİ »óÅÂ");

        _characterStats.EquipItem(Sword);
        PrintStats("¹«±â ÀåÂø");

        _characterStats.EquipItem(Shield);
        PrintStats("°©¿Ê ÀåÂø");

        _characterStats.UnequipItem(Shield);
        PrintStats("°©¿Ê ÇØÁ¦");
    }

    private void PrintStats(string context)
    {
        Debug.Log($"{context} Stats:");
        foreach (var stat in BaseStats)
        {
            Debug.Log($"{stat.StatType}: {_characterStats.GetStatValue(stat)}");
        }
    }
}