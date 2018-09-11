using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour 
{
    [Header("Weapon Stats")]
    [SerializeField] [Range(1, 100)] float damage;
    [SerializeField] [Range(1, 1000)] float range;
    [SerializeField] [Range(1, 12)] float fireRate;
    [SerializeField] [Range(1, 1000)] float impactForce;
    [SerializeField] [Range(1, 100)] int magSize;
    [SerializeField] [Range(1, 1000)] int maxAmmo;
    [SerializeField] [Range(1, 10)] int regularSwayLevel;
    [SerializeField] [Range(1, 10)] int recoilSwayLevel;
    [SerializeField] [Range(0, 5)] float recoilDuration;
    [Header("Weapon Anmimations")]
    [SerializeField] AnimationClip shootAnimation;
    [SerializeField] AnimationClip reloadAnimation;
    [Header("Weapon Audio Sources")]
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource emptyMagSound;
    [Header("Weapon Events")]
    [SerializeField] UnityEvent onShot;
    [SerializeField] UnityEvent onReload;
    [SerializeField] UnityEvent onEmptyMag;
    [SerializeField] UnityEvent onCrosshairScale;
    const float baseSway = 0.01f;
    Transform fpsCamera;
    ParticleSystem muzzleFlash;
    float lastFireTime = 0;
    int bulletsInMag = 0;
    int ammoLeft = 0;
    float regularSway = 0;
    float recoilSway = 0;
    bool isReloading = false;
    float crosshairScaling = 1;
    int consecutiveShots = 0;

    void Awake()
    {
        fpsCamera = GetComponentInParent<Camera>().transform;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        
        bulletsInMag = magSize;
        ammoLeft = maxAmmo;
        regularSway = baseSway * regularSwayLevel;
        recoilSway = baseSway * recoilSwayLevel;
        recoilDuration += 1 / fireRate;
    }

    void OnDisable()
    {
        crosshairScaling = 1;
        consecutiveShots = 0;
        lastFireTime = 0;
    }

    void Update() 
	{
        if (lastFireTime < Time.time - recoilDuration)
        {
            if (consecutiveShots != 0)
                consecutiveShots = 0;
            crosshairScaling = Mathf.Lerp(crosshairScaling, 1, recoilDuration);
            onCrosshairScale.Invoke();
        }

        if (InputManager.Instance.GetFireButton() && CanShoot())
        {
            if (bulletsInMag > 0)
            {
                Shoot();
                onShot.Invoke();
            }
           else
                onEmptyMag.Invoke();
        }

        if (InputManager.Instance.GetReloadButton() && CanReload())
            StartCoroutine(Reload());
	}

    void Shoot()
    {
        muzzleFlash.Play();

        Vector3 sway;
        float horSway;
        float verSway;

        if (lastFireTime < Time.time - recoilDuration)
        {
            horSway = Random.Range(-regularSway, regularSway);
            verSway = Random.Range(-regularSway, regularSway);
        }
        else
        {
            horSway = Random.Range(-recoilSway, recoilSway);
            verSway = Random.Range(-recoilSway, recoilSway);
            consecutiveShots++;
        }

        sway = new Vector3(horSway, verSway, 0);
        crosshairScaling = 1 + recoilSway * consecutiveShots;
        onCrosshairScale.Invoke();
        
        lastFireTime = Time.time;
        bulletsInMag--;

        RaycastHit hit;
        
        if (Physics.Raycast(fpsCamera.position, (fpsCamera.forward + sway).normalized, out hit, range))
        {
            Life targetLife = hit.transform.GetComponent<Life>();
            Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();

            if (targetLife)
            {
                float damagePercentage = 1 - (hit.transform.position - transform.position).sqrMagnitude / (range * range);
                targetLife.TakeDamage(damage * damagePercentage);
            }
            if (targetRigidbody)
            {
                float impactPercentage = 1 - (hit.transform.position - transform.position).sqrMagnitude / (range * range);
                targetRigidbody.AddForceAtPosition(-hit.normal * impactForce * impactPercentage, hit.point);
            }
        }
    }

    IEnumerator Reload()
    { 
        isReloading = true;
        
        int bulletsInMagAfterReload = 0;
        int ammoLeftAfterReload = 0;
        
        if (ammoLeft > magSize)
        {
            if (bulletsInMag > 0)
                bulletsInMagAfterReload = magSize + 1;
            else
                bulletsInMagAfterReload = magSize;
            ammoLeftAfterReload = ammoLeft - magSize;
        }
        else
        {
            bulletsInMagAfterReload = ammoLeft;
            ammoLeftAfterReload = 0;
        }
        
        bulletsInMag = bulletsInMagAfterReload;
        ammoLeft = ammoLeftAfterReload;

        onReload.Invoke();
        yield return new WaitForSeconds(reloadAnimation.length);
       
        isReloading = false;
    }

    bool CanShoot()
    {
        return !isReloading && Time.time >= lastFireTime + 1 / fireRate && !PauseMenu.IsPaused && !LevelManager.Instance.GameOver;
    }

    bool CanReload()
    {
        return !isReloading && bulletsInMag < magSize + 1 && ammoLeft > 0 && Time.time >= lastFireTime + 1 / fireRate &&
                !PauseMenu.IsPaused && !LevelManager.Instance.GameOver;
    }

    public bool HasFinishedFiring()
    {
        return consecutiveShots == 0;
    }

    public int BulletsInMag
    {
        get { return bulletsInMag; }
    }

    public int AmmoLeft
    {
        get { return ammoLeft; }
        set{
            ammoLeft += value;
        }
    }

    public int MagSize
    {
        get { return magSize; }
    }

    public bool IsReloading
    {
        get { return isReloading; }
    }

    public float CrosshairScaling
    {
        get { return crosshairScaling; }
    }

    public AnimationClip ShootAnimation
    {
        get { return shootAnimation; }
    }

    public AnimationClip ReloadAnimation
    {
        get { return reloadAnimation; }
    }

    public AudioSource ShootSound
    {
        get { return shootSound; }
    }
    public AudioSource ReloadSound
    {
        get { return reloadSound; }
    }

    public AudioSource EmptyMagSound
    {
        get { return emptyMagSound; }
    }

    public UnityEvent OnShot
    {
        get { return onShot; }
    }

    public UnityEvent OnReload
    {
        get { return onReload; }
    }

    public UnityEvent OnEmptyMag
    {
        get { return onEmptyMag; }
    }

    public UnityEvent OnCrossHairScale
    {
        get { return onCrosshairScale; }
    }
}
