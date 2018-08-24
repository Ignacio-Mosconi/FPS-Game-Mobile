using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WeaponReloading))]

public class WeaponShooting : MonoBehaviour 
{
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float fireRate;
    [SerializeField] float impactForce;
    [SerializeField] UnityEvent onShot;
    [SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource emptyMagSound;
    [SerializeField] AnimationClip shootAnimation;
    WeaponReloading weaponReloading;
    Camera fpsCamera;
    ParticleSystem muzzleFlash;
    float nextFireTime = 0;

    void Awake()
    {
        weaponReloading = GetComponent<WeaponReloading>();
        fpsCamera = GetComponentInParent<Camera>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    void Update() 
	{
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !PauseMenu.IsPaused && !LevelManager.Instance.GameOver)
        {
            if (weaponReloading.BulletsInMag > 0)
            {
                nextFireTime = Time.time + 1 / fireRate;
                Shoot();
                onShot.Invoke();
            }
           else
               if (!emptyMagSound.isPlaying)
                   emptyMagSound.Play();
        }
	}

    void Shoot()
    {
        muzzleFlash.Play();
        shotSound.Play();

        weaponReloading.BulletsInMag--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Life targetLife = hit.transform.GetComponent<Life>();
            Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();

            if (targetLife)
            {
                float damagePercentage = 1 - (hit.transform.position - transform.position).magnitude / range;
                targetLife.TakeDamage(damage * damagePercentage);
            }
            if (targetRigidbody)
                targetRigidbody.AddForce(-hit.normal * impactForce);
        }
    }

    public float FireRate
    {
        get { return fireRate; }
    }

    public AnimationClip ShootAnimation
    {
        get { return shootAnimation; }
    }

    public UnityEvent OnShot
    {
        get { return onShot; }
    }
}
