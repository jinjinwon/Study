using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;                                           // 아이템 타입
    public EquipType EquipType;                                         // 아이템 장착 부위
    public string ItemName;                                             // 아이템 이름
    [TextArea(3, 5)] public string Description;                         // 아이템 설명
    public Sprite Icon;                                                 // 아이템 아이콘
    public bool IsStackable;                                            // 중첩 가능 여부
    public int MaxStackSize;                                            // 최대 중첩 개수
    public List<StatModifier> StatModifiers = new List<StatModifier>(); // 스탯 수정 리스트
}
