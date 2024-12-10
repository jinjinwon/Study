using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Dictionary<Stat, float> _stats = new Dictionary<Stat, float>();
    private List<ItemData> _equippedItems = new List<ItemData>();

    public int StatusPoints { get; private set; } = 10;                 // �ʱ� �������ͽ� ����Ʈ

    public event Action<Stat, float> OnStatChanged;                     // ���� ���� �̺�Ʈ
    public event Action<int> OnStatusPointsChanged;                     // �������ͽ� ����Ʈ ���� �̺�Ʈ

    public CharacterStats(List<Stat> baseStats)
    {
        foreach (var stat in baseStats)
        {
            _stats[stat] = stat.DefaultValue; // �⺻�� ����
        }
    }

    // ���� ���� ��������
    public float GetStatValue(Stat stat)
    {
        if (_stats.ContainsKey(stat))
            return _stats[stat];
        return 0; // �ش� ������ ������ 0 ��ȯ
    }

    // ������ ����
    public void EquipItem(ItemData item)
    {
        _equippedItems.Add(item);
        ApplyModifiers(item.StatModifiers);
    }

    // ������ ����
    public void UnequipItem(ItemData item)
    {
        _equippedItems.Remove(item);
        RemoveModifiers(item.StatModifiers);
    }

    // ���� ���� ����
    private void ApplyModifiers(List<StatModifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            if (_stats.ContainsKey(modifier.TargetStat))
            {
                _stats[modifier.TargetStat] += modifier.ModifierValue;
            }
            else
            {
                _stats[modifier.TargetStat] = modifier.ModifierValue;
            }
        }
    }

    // ���� ���� ����
    private void RemoveModifiers(List<StatModifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            if (_stats.ContainsKey(modifier.TargetStat))
            {
                _stats[modifier.TargetStat] -= modifier.ModifierValue;

                // �⺻�� ���Ϸ� �������� �⺻ �� ǥ��
                if (_stats[modifier.TargetStat] < modifier.TargetStat.DefaultValue)
                {
                    _stats[modifier.TargetStat] = modifier.TargetStat.DefaultValue;
                }
            }
        }
    }

    // �ɷ�ġ ����
    public void IncreaseStatWithPoints(StatModifier stat)
    {
        if (StatusPoints > 0)
        {
            _stats[stat.TargetStat] += stat.ModifierValue;
            StatusPoints -= 1;

            OnStatChanged?.Invoke(stat.TargetStat, _stats[stat.TargetStat]);
            OnStatusPointsChanged?.Invoke(StatusPoints);
        }
    }

    // �ɷ�ġ ����
    public void DecreaseStatWithPoints(StatModifier stat)
    {
        float minValue = GetMinimumStatValue(stat.TargetStat);

        if (_stats[stat.TargetStat] > minValue) 
        {
            _stats[stat.TargetStat] -= stat.ModifierValue;
            StatusPoints += 1;

            OnStatChanged?.Invoke(stat.TargetStat, _stats[stat.TargetStat]);
            OnStatusPointsChanged?.Invoke(StatusPoints);
        }
    }

    // �ּҰ� ���
    private float GetMinimumStatValue(Stat stat)
    {
        float baseValue = stat.DefaultValue;
        float itemBonus = GetItemBonus(stat);
        return baseValue + itemBonus;
    }

    // ���������� ���� ���� ���ʽ� ���
    private float GetItemBonus(Stat stat)
    {
        float totalBonus = 0;
        foreach (var item in _equippedItems)
        {
            foreach (var modifier in item.StatModifiers)
            {
                if (modifier.TargetStat == stat)
                {
                    totalBonus += modifier.ModifierValue;
                }
            }
        }
        return totalBonus;
    }
}
