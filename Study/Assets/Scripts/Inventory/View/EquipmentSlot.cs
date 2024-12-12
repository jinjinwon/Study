using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : SlotData
{
    [SerializeField]
    private EquipType equipType; // ������ ���� ������ ������ �� �ִ� �ʵ�
    public EquipType EquipType => equipType; // �ܺο��� �б� �������� ���� ����
}
