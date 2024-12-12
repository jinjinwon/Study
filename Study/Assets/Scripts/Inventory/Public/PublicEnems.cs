/// <summary>
/// 장비 아이템 장착 부위
/// </summary>
public enum EquipType   
{
    None = 0,
    Helmet,             // 헬멧
    Armor,              // 갑옷
    Weapon_1,           // 무기 1
    Weapon_2,           // 무기 2
    Weapon_3,           // 무기 3
    Spacial,            // 특수 아이템
    Ring,               // 반지
    Necklace,           // 목걸이
}

/// <summary>
/// 아이템 타입
/// </summary>
public enum ItemType
{
    Equipment,             // 장비
    Spacial,               // 아직 고민 중
    Food,                  // 음식
    Etc,                   // 기타
}

/// <summary>
/// 스텟 타입
/// </summary>
public enum StatType
{
    Attack,                 // 공격력
    Defence,                // 방어력
    WorkSpeed,              // 작업 속도    
    Weight,                 // 소지 중량
    HP,                     // 체력
    Stamina                 // 기력
}

public enum SortType
{
    ByName,        // 이름 순
    ByQuantity,    // 수량 순
    ByItemType,    // 아이템 타입 순
}