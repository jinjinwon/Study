using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Dictionary<Stat, float> _stats = new Dictionary<Stat, float>();
    private List<ItemData> _equippedItems = new List<ItemData>();

    public int StatusPoints { get; private set; } = 10;                 // 초기 스테이터스 포인트

    public event Action<Stat, float> OnStatChanged;                     // 스탯 변경 이벤트
    public event Action<int> OnStatusPointsChanged;                     // 스테이터스 포인트 변경 이벤트

    public CharacterStats(List<Stat> baseStats)
    {
        foreach (var stat in baseStats)
        {
            _stats[stat] = stat.DefaultValue; // 기본값 설정
        }
    }

    // 현재 스탯 가져오기
    public float GetStatValue(Stat stat)
    {
        if (_stats.ContainsKey(stat))
            return _stats[stat];
        return 0; // 해당 스탯이 없으면 0 반환
    }

    // 아이템 장착
    public void EquipItem(ItemData item)
    {
        _equippedItems.Add(item);
        ApplyModifiers(item.StatModifiers);
    }

    // 아이템 해제
    public void UnequipItem(ItemData item)
    {
        _equippedItems.Remove(item);
        RemoveModifiers(item.StatModifiers);
    }

    // 스탯 수정 적용
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

    // 스탯 수정 제거
    private void RemoveModifiers(List<StatModifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            if (_stats.ContainsKey(modifier.TargetStat))
            {
                _stats[modifier.TargetStat] -= modifier.ModifierValue;

                // 기본값 이하로 내려가면 기본 값 표시
                if (_stats[modifier.TargetStat] < modifier.TargetStat.DefaultValue)
                {
                    _stats[modifier.TargetStat] = modifier.TargetStat.DefaultValue;
                }
            }
        }
    }

    // 능력치 증가
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

    // 능력치 감소
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

    // 최소값 계산
    private float GetMinimumStatValue(Stat stat)
    {
        float baseValue = stat.DefaultValue;
        float itemBonus = GetItemBonus(stat);
        return baseValue + itemBonus;
    }

    // 아이템으로 인한 스탯 보너스 계산
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
