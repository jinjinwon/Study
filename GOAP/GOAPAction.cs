// GOAPAction.cs
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

    public abstract bool CheckProceduralPrecondition(GameObject agent);
    public abstract bool Perform(GameObject agent);
    public abstract bool IsDone();
    public abstract bool RequiresInRange();
    public bool IsInRange() => inRange;
}
