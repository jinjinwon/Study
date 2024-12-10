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

        PrintStats("�ʱ� ���� ����");

        _characterStats.EquipItem(Sword);
        PrintStats("���� ����");

        _characterStats.EquipItem(Shield);
        PrintStats("���� ����");

        _characterStats.UnequipItem(Shield);
        PrintStats("���� ����");
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