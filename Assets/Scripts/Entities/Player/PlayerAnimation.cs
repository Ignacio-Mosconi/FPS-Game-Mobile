using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour 
{
    [SerializeField] UnityEvent onShootingEnabledToggle;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    [SerializeField] AnimationClip[] longGunAnimations;
    [SerializeField] AnimationClip[] handGunAnimations;
    Animator animator;
    CharacterController charController;
    PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        charController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        weaponManager.OnWeaponSwap.AddListener(ChangeWeaponAnimations);
        weaponManager.PlayerShooting.OnShot.AddListener(HasShot);
        weaponManager.PlayerReloading.OnReload.AddListener(HasReloaded);

        ChangeWeaponAnimations();
    }

    void Update() 
	{
        Vector3 horVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);
        float normalizedVelocity = horVelocity.magnitude / playerMovement.MovementSpeed;
        float verticalVelocity = charController.velocity.y;
        bool jumping = playerMovement.IsJumping();

        if (!jumping && !animator.GetBool("Is Reloading"))
        {
            if (normalizedVelocity < 0.6)
            {
                if (!weaponManager.PlayerShooting.enabled)
                    EnableShooting();
                if (!weaponManager.PlayerReloading.enabled)
                    EnableReloading();
            }
            else
            {
                if (normalizedVelocity >= 0.6)
                {
                    if (weaponManager.PlayerShooting.enabled)
                        DisableShooting();
                    if (weaponManager.PlayerReloading.enabled)
                        DisableReloading();
                }
            }
        }
        else
        {
            if (weaponManager.PlayerShooting.enabled)
                DisableShooting();
            if (weaponManager.PlayerReloading.enabled)
                DisableReloading();
        }

        animator.SetFloat("Horizontal Velocity", normalizedVelocity, 0.2f, Time.deltaTime);
        animator.SetFloat("Vertical Velocity", verticalVelocity, 0.2f, Time.deltaTime);
        animator.SetBool("Is Jumping", jumping);
	}

    void HasShot()
    {
        animator.SetBool("Is Shooting", true);
        Invoke("IsNotShooting", 1 / weaponManager.PlayerShooting.FireRate);
    }

    void IsNotShooting()
    {
        animator.SetBool("Is Shooting", false);
    }

    void HasReloaded()
    {
        animator.SetBool("Is Reloading", true);
        Invoke("IsNotReloading", weaponManager.PlayerReloading.ReloadTime);
    }

    void IsNotReloading()
    {
        animator.SetBool("Is Reloading", false);
    }

    void DisableShooting()
    {
        weaponManager.PlayerShooting.enabled = false;
        onShootingEnabledToggle.Invoke();
    }

    void EnableShooting()
    {
        weaponManager.PlayerShooting.enabled = true;
        onShootingEnabledToggle.Invoke();
    }

    void DisableReloading()
    {
        weaponManager.PlayerReloading.enabled = false;
    }

    void EnableReloading()
    {
        weaponManager.PlayerReloading.enabled = true;
    }

    void ChangeWeaponAnimations()
    {
        animator.runtimeAnimatorController = animatorOverrideController;
        animatorOverrideController["AK-47 Reloading"] = weaponManager.PlayerReloading.ReloadAnimation;

        switch (weaponManager.GetCurrentWeaponIndex())
        {
            case 0:
                animatorOverrideController["Assault Rifle Idle"] = longGunAnimations[0];
                animatorOverrideController["Assault Rifle Jumping"] = longGunAnimations[1];
                animatorOverrideController["Assault Rifle Shooting"] = longGunAnimations[2];
                animatorOverrideController["Assault Rifle Sprinting"] = longGunAnimations[3];
                animatorOverrideController["Assault Rifle Walking"] = longGunAnimations[4]; 
                break;
            case 1:
                animatorOverrideController["Assault Rifle Idle"] = handGunAnimations[0];
                animatorOverrideController["Assault Rifle Jumping"] = handGunAnimations[1];
                animatorOverrideController["Assault Rifle Shooting"] = handGunAnimations[2];
                animatorOverrideController["Assault Rifle Sprinting"] = handGunAnimations[3];
                animatorOverrideController["Assault Rifle Walking"] = handGunAnimations[4];
                break;
        }

    }

    public UnityEvent OnShootingEnabledToggle
    {
        get { return onShootingEnabledToggle; }
    }
}
