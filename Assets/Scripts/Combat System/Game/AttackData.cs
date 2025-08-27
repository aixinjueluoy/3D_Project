using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat System/Creat a new Attack")]
public class AttackDate : ScriptableObject
{
    [field:SerializeField] public string AttackAnimName { get; private set; }
    [field:SerializeField] public float ImpactStartTime { get; private set; }
    [field:SerializeField] public float ImpactEndTime { get; private set; }
}
