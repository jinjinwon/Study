using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Public/Stat")]
public class Stat : ScriptableObject
{
    public StatType StatType;  
    public float DefaultValue;
}
