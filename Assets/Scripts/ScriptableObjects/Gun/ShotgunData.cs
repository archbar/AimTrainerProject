using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShotgunData", menuName = "GunType/ShotgunData")]
public class ShotgunData : GunData
{
    [Tooltip("The spread or deviation of bullets when fired.")]
    public float spread;

    [Tooltip("Number of bullets fired on each shot")]
    public int bulletsPerShot;

    [Tooltip("The multiplier for decreasing damage over distance")]
    public float damageDropOff;

    [Tooltip("Distance that damage starts to decrease at")]
    public float damageDropOffDistance;

    // some kind of value to indicate spray pattern
}
