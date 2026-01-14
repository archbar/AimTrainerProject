using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;


public class FPSGunController : MonoBehaviour
{
    #region Fields
    [SerializeField] private GunData[] loadout;
    public static FPSGunController Instance { get; private set; }
    private GunData gunData;

    private float currentcooldown;
    
    private int currentIndex;
    private int layerMask; 

    // Booleans
    private bool gunInhand;
    private bool shooting;
    private bool readyToShoot;

    private int currentWeaponIndex;

    //shotgun
    [SerializeField] int inaccuracyDistance;


    // Unity Components
    public Transform weaponParent;
    private Transform gunTip;
    private Camera mainCamera;
    ParticleSystem muzzleFlash;
    // Other Components
    FPSCameraController cameraData;

    [SerializeField] private GameObject bulletholePrefab;
    private GameObject currentWeapon;
    [SerializeField] private GameObject bulletPrefab;

    //burstfire    
    [SerializeField] private int currentBurstCount;

    private GunData[] originalLoadout;


    #endregion
    #region UnityMethods    

    void Awake()
    {
        gunData = loadout[0];

        mainCamera = GetComponent<Camera>();
        cameraData = GetComponent<FPSCameraController>();
        layerMask = LayerMask.GetMask("Target", "ShootableSurface", "Ground"); // Add the "CanBeShot" layer to the layerMask
        readyToShoot = true;
    }
    void Update()
    {    
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput > 0f)
        {
            // Scroll up, cycle to the next weapon
            CycleWeapon(1);
        }
        else if (scrollWheelInput < 0f)
        {
            // Scroll down, cycle to the previous weapon
            CycleWeapon(-1);
        }

        // Weapon selection using number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(0);
            gunInhand = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(1);
            gunInhand = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(2);
            gunInhand = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Equip(3);
            gunInhand = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Equip(4);
            gunInhand = true;
        }

        if (gunInhand == true)
        {
            RifleData rifleData = gunData as RifleData;

            if (rifleData != null)
            {
                if (rifleData.automatic)
                {
                    shooting = Input.GetKey(KeyCode.Mouse0);
                }
                else if (rifleData.canBurst)
                {
                    HandleBurstShooting();
                }
                else
                {
                    // Add logic for non-automatic, non-burst rifles
                    shooting = Input.GetKeyDown(KeyCode.Mouse0);
                }
            }
            else
            {
                // Add logic for non-rifle guns
                shooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && shooting)
            {
                Shoot();
            }

            //recoil
            //weapon position elasticity
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            //cooldown
            if (currentcooldown > 0) 
            { 
                currentcooldown -= Time.deltaTime; 
            }
            //  1/how many shots you want per second = how long it will take to reset
        }
        if (currentWeapon != null) 
        { 
            Aim(Input.GetMouseButton(1)); 
        }
    }

    #endregion
    #region NonUnityMethods
    private void Shoot()
    {
        if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsFired++; }
        readyToShoot = false;

        if (gunData is ShotgunData shotgunData)
        {          
            for (int i = 0; i < shotgunData.bulletsPerShot; i++)
            {
                var hitSuccess = Physics.Raycast(new Ray(transform.position, GetShootingDirection()), out RaycastHit hit, gunData.range, layerMask);
                if (!hitSuccess) 
                {
                    if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsMissed++; }
                    return; 
                }

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
                {
                    hit.collider.SendMessage("TakeDamage", gunData.damage, SendMessageOptions.DontRequireReceiver);
                    if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsHit++; }
                } else
                {
                    GameObject t_newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
                    t_newHole.transform.LookAt(hit.point + hit.normal);
                    if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsMissed++; }
                    Destroy(t_newHole, 2f);
                }
                //bullet coming out of gun
                GameObject b = GameObject.Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
                b.GetComponent<BulletBehaviour>().SetTarget(hit.point, gunData.ammoSpeed);
                b.SetActive(true);
            }
        } else
        {
            var hitSuccess = Physics.Raycast(new Ray(transform.position, GetShootingDirection()), out RaycastHit hit, gunData.range, layerMask);
            if (!hitSuccess)
            {
                if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsMissed++; }
                return;
            }
            //if bullet hits a target
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                hit.collider.SendMessage("TakeDamage", gunData.damage, SendMessageOptions.DontRequireReceiver);
                if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsHit++; }
            }
            //if bullet doesnt hit a target
            else
            {
                GameObject t_newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
                t_newHole.transform.LookAt(hit.point + hit.normal);
                if (GameManager.Instance.taskPlaying) { GameManager.Instance.noOfShotsMissed++; }
                Destroy(t_newHole, 2f);
            }
            //bullet coming out of gun
            GameObject b = GameObject.Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
            b.GetComponent<BulletBehaviour>().SetTarget(hit.point, gunData.ammoSpeed);
            b.SetActive(true);
        }

        //muzzle flash
        muzzleFlash.Play();

        //camera recoil
        cameraData.RecoilFire(Input.GetMouseButton(1));

        //prefab recoil and kickback
        currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;

        //cooldown        
        Invoke("ResetShot", gunData.fireRate);
        

        if (gunData is RifleData rifleData && rifleData.canBurst)
        {
            StartCoroutine(ShotCooldown());
        }

        GameManager.Instance.UpdateScore();
    }


    void Equip(int p_ind)
    {
        if (currentWeapon != null) Destroy(currentWeapon);
        currentIndex = p_ind;

        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent);

        gunData = loadout[p_ind];
        cameraData.SetRecoilValues(gunData.returnSpeed, gunData.snappiness, gunData.recoilX, gunData.recoilY, gunData.recoilZ, gunData.aimRecoilX, gunData.aimRecoilY, gunData.aimRecoilZ);

        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.eulerAngles = Vector3.zero;
        // allows the gun to be pulled out to the correct place on the screen based on the cameras orientation
        t_newWeapon.transform.rotation = Camera.main.transform.rotation; 
        // Really stupid, means the particle system has to be the 3rd child of the 1st child of the t_newWeapon gameobject
        gunTip = t_newWeapon.transform.GetChild(0).GetChild(2);
        muzzleFlash = gunTip.gameObject.GetComponent<ParticleSystem>();
        currentWeapon = t_newWeapon;
    }
    // will be how the player can ads
    void Aim(bool p_isAiming)
    {
        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

        if (p_isAiming)
        {
            // Aim
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            // Hip
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);            
        }    
    }

    void ResetShot() 
    {
        readyToShoot = true;
    }

        Vector3 GetShootingDirection()
        {
          Vector3 targetPos = transform.position + transform.forward * gunData.range;
          if (gunData is ShotgunData shotgunData) targetPos = new Vector3(
                targetPos.x + Random.Range(-shotgunData.spread, shotgunData.spread),
                targetPos.y + Random.Range(-shotgunData.spread, shotgunData.spread),
                targetPos.z + Random.Range(-shotgunData.spread, shotgunData.spread));

            Vector3 direction = targetPos - transform.position;
            return direction.normalized;
        }


    void HandleBurstShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToShoot)
        {
            if (gunData is RifleData rifleData)
            {
                currentBurstCount = rifleData.numberOfShotsPerBurst; 
                StartCoroutine(BurstShoot());
            }
        }
    }

    IEnumerator BurstShoot()
    {
        while (currentBurstCount > 0)
        {
            Shoot();
            currentBurstCount--;

            if (currentBurstCount > 0)
            {
                if (gunData is RifleData rifleData)
                {
                    yield return new WaitForSeconds(rifleData.burstgap);
                }
            }
        }
    }

    IEnumerator ShotCooldown()
    {
        if (gunData is RifleData rifleData)
        {
            yield return new WaitForSeconds(rifleData.burstgap);
        }
        
        readyToShoot = true;
    }
    //cycle weapons with scroll wheel
    void CycleWeapon(int direction)
    {
        // Adjust the current weapon index based on the direction
        currentWeaponIndex += direction;

        // Ensure the index is within valid bounds
        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, loadout.Length - 1);

        // Equip the weapon at the new index
        Equip(currentWeaponIndex);

        gunInhand = true;
    }
    public void ResetGunController()
    {
        // Reset variables to their initial state
        currentcooldown = 0f;
        currentIndex = 0;
        gunInhand = false;
        shooting = false;
        readyToShoot = true;
        currentWeaponIndex = 0;
        currentBurstCount = 0;

        // Reset the loadout to its original state
        loadout = originalLoadout.Clone() as GunData[];

        // Destroy the current weapon
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Call Equip to instantiate the default weapon
        Equip(0);
    }


    #endregion
}









