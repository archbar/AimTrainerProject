using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "GunType/GunData")]
public class GunData : ScriptableObject
{
    [Tooltip("Physical element of the gun")]
    public GameObject prefab;

    [Tooltip("The rate of fire of the gun in shots per second.")]
    public float fireRate;

    [Tooltip("The recoil value for the gun transform, actualises kickback")]
    public float recoil;

    [Tooltip("The amount of x,y,z axis recoil experienced after each shot.")]
    public float recoilX, recoilY, recoilZ;

    [Tooltip("The amount of x,y,z axis recoil experienced after each shot while in ADS.")]
    public float aimRecoilX, aimRecoilY, aimRecoilZ;

    [Tooltip("How fast the gun resets after said recoil.")]
    public float returnSpeed, snappiness;

    [Tooltip("The damage inflicted by each bullet.")]
    public float damage;

    [Tooltip("The range of the gun.")]
    public float range;
    
    [Tooltip("The speed our bullets travel at in m/s.")]
    public float ammoSpeed;

    [Tooltip("How fast the gun will aim in.")]
    public float aimSpeed;
    
    [Tooltip("How much the gun kicks back.")]
    public float kickback;

}
