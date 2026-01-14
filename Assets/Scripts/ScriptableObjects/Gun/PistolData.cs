using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPistolData", menuName = "GunType/PistolData")]
public class PistolData : GunData
{
    [Tooltip("If it has a suppresor.")]
    public bool hasSuppressor;
}
