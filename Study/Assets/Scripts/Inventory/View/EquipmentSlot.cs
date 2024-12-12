using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : SlotData
{
    [SerializeField]
    private EquipType equipType; // 슬롯의 장착 부위를 설정할 수 있는 필드
    public EquipType EquipType => equipType; // 외부에서 읽기 전용으로 접근 가능
}
