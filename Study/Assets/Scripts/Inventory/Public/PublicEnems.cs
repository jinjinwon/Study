/// <summary>
/// ��� ������ ���� ����
/// </summary>
public enum EquipType   
{
    None = 0,
    Helmet,             // ���
    Armor,              // ����
    Weapon_1,           // ���� 1
    Weapon_2,           // ���� 2
    Weapon_3,           // ���� 3
    Spacial,            // Ư�� ������
    Ring,               // ����
    Necklace,           // �����
}

/// <summary>
/// ������ Ÿ��
/// </summary>
public enum ItemType
{
    Equipment,             // ���
    Spacial,               // ���� ��� ��
    Food,                  // ����
    Etc,                   // ��Ÿ
}

/// <summary>
/// ���� Ÿ��
/// </summary>
public enum StatType
{
    Attack,                 // ���ݷ�
    Defence,                // ����
    WorkSpeed,              // �۾� �ӵ�    
    Weight,                 // ���� �߷�
    HP,                     // ü��
    Stamina                 // ���
}

public enum SortType
{
    ByName,        // �̸� ��
    ByQuantity,    // ���� ��
    ByItemType,    // ������ Ÿ�� ��
}