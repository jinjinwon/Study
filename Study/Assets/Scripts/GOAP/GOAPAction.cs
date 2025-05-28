using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    public float cost = 1f;
    public bool inRange = false;
    public float actionRange = 1.5f;
    public Dictionary<string, object> preconditions = new Dictionary<string, object>();
    public Dictionary<string, object> effects = new Dictionary<string, object>();

    public void AddPrecondition(string key, object value) => preconditions[key] = value;
    public void AddEffect(string key, object value) => effects[key] = value;

    // 런타임에 목표 오브젝트 탐색 등 동적 검증
    public abstract bool CheckProceduralPrecondition(GameObject agent);
    // 실제 액션 수행 로직
    public abstract bool Perform(GameObject agent);
    // 완료 여부 반환
    public abstract bool IsDone();
    // 사거리 기반 이동이 필요한지 여부
    public abstract bool RequiresInRange();
}
