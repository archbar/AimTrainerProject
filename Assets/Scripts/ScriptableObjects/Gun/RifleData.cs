using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRifleData", menuName = "GunType/RifleData")]
public class RifleData : GunData
{
    [Tooltip("Whether the gun is automatic or semi-automatic.")]
    public bool automatic;

    [Tooltip("Whether the gun can burst fire or not")]
    public bool canBurst;

    [Tooltip("Gap inbetween each burst.")]
    public float burstgap;

    [Tooltip("Number of shots fired in a burst")]
    public int numberOfShotsPerBurst;
}
