using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private EquipmentView equipmentView;
    [SerializeField] private ItemRemoveHandler itemRemoveHandler;

    [SerializeField] private int inventoryMaxSlots = 20; // 인벤토리 최대 슬롯 수 설정

    private InventoryModel _inventoryModel;
    private EquipmentModel _equipmentModel;
    private InventoryPresenter _inventoryPresenter;
    private EquipmentPresenter _equipmentPresenter;

    void Start()
    {
        _inventoryModel = new InventoryModel(inventoryMaxSlots);
        _equipmentModel = new EquipmentModel();

        _inventoryPresenter = new InventoryPresenter(_inventoryModel, inventoryView, null);
        _equipmentPresenter = new EquipmentPresenter(_equipmentModel, equipmentView, _inventoryPresenter);

        // InventoryPresenter에 EquipmentPresenter를 설정
        _inventoryPresenter.SetEquipmentPresenter(_equipmentPresenter);

        inventoryView.InitSlot(_inventoryPresenter);
        equipmentView.InitSlot(_inventoryPresenter, _equipmentPresenter);

        itemRemoveHandler.Initialize(_inventoryPresenter);
    }
}
