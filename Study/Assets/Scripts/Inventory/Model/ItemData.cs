using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;                                           // ������ Ÿ��
    public EquipType EquipType;                                         // ������ ���� ����
    public string ItemName;                                             // ������ �̸�
    [TextArea(3, 5)] public string Description;                         // ������ ����
    public Sprite Icon;                                                 // ������ ������
    public bool IsStackable;                                            // ��ø ���� ����
    public int MaxStackSize;                                            // �ִ� ��ø ����
    public List<StatModifier> StatModifiers = new List<StatModifier>(); // ���� ���� ����Ʈ
}
