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
    [SerializeField] [Range(1, 1000)] int ammoLeft;
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
    Transform fpsCamera;
    ParticleSystem muzzleFlash;
    float nextFireTime = 0;
    int bulletsInMag = 0;
    bool isReloading = false;

    void Awake()
    {
        fpsCamera = GetComponentInParent<Camera>().transform;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        
        bulletsInMag = magSize;
    }

    void Update() 
	{
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

        nextFireTime = Time.time + 1 / fireRate;
        bulletsInMag--;

        RaycastHit hit;
        
        if (Physics.Raycast(fpsCamera.position, fpsCamera.forward, out hit, range))
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
        return (!isReloading && Time.time >= nextFireTime && !PauseMenu.IsPaused && !LevelManager.Instance.GameOver);
    }

    bool CanReload()
    {
        return (!isReloading && bulletsInMag < magSize + 1 && ammoLeft > 0) && !PauseMenu.IsPaused && !LevelManager.Instance.GameOver;
    }

    public int BulletsInMag
    {
        get { return bulletsInMag; }
    }

    public int AmmoLeft
    {
        get { return ammoLeft; }
    }

    public bool IsReloading
    {
        get { return isReloading; }
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
}
