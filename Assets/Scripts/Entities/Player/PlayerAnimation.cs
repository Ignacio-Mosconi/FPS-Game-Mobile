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
        foreach (Transform weapon in weaponManager.transform)
        {
            weapon.gameObject.GetComponent<WeaponShooting>().OnShot.AddListener(HasShot);
            weapon.gameObject.GetComponent<WeaponReloading>().OnReload.AddListener(HasReloaded);
        }

        ChangeWeaponAnimations();
    }

    void Update() 
	{
        Vector3 horVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);
        float normalizedVelocity = horVelocity.magnitude / playerMovement.MovementSpeed;
        float verticalVelocity = charController.velocity.y;
        bool jumping = playerMovement.IsJumping();

        if (!jumping && !animator.GetBool("Is Reloading") && normalizedVelocity < 0.6)
        {
            if (!weaponManager.CurrentWeaponShooting.enabled)
                EnableShooting();
            if (!weaponManager.CurrentWeaponReloading.enabled)
                EnableReloading();
        }
        else
        {
            if (weaponManager.CurrentWeaponShooting.enabled)
                DisableShooting();
            if (weaponManager.CurrentWeaponReloading.enabled)
                DisableReloading();
        }

        animator.SetFloat("Horizontal Velocity", normalizedVelocity, 0.2f, Time.deltaTime);
        animator.SetFloat("Vertical Velocity", verticalVelocity, 0.2f, Time.deltaTime);
        animator.SetBool("Is Jumping", jumping);
	}

    void HasShot()
    {
        animator.SetTrigger("Has Shot");
    }

    void HasReloaded()
    {
        animator.SetBool("Is Reloading", true);
        Invoke("IsNotReloading", weaponManager.CurrentWeaponReloading.ReloadTime);
    }

    void IsNotReloading()
    {
        animator.SetBool("Is Reloading", false);
    }

    void DisableShooting()
    {
        weaponManager.CurrentWeaponShooting.enabled = false;
        onShootingEnabledToggle.Invoke();
    }

    void EnableShooting()
    {
        weaponManager.CurrentWeaponShooting.enabled = true;
        onShootingEnabledToggle.Invoke();
    }

    void DisableReloading()
    {
        weaponManager.CurrentWeaponReloading.enabled = false;
    }

    void EnableReloading()
    {
        weaponManager.CurrentWeaponReloading.enabled = true;
    }

    void ChangeWeaponAnimations()
    {
        animator.runtimeAnimatorController = animatorOverrideController;
        animatorOverrideController["DEFAULT SHOOTING"] = weaponManager.CurrentWeaponShooting.ShootAnimation;
        animatorOverrideController["DEFAULT RELOADING"] = weaponManager.CurrentWeaponReloading.ReloadAnimation;

        switch (weaponManager.GetCurrentWeaponIndex())
        {
            case 0:
                animatorOverrideController["DEFAULT IDLE"] = longGunAnimations[0];
                animatorOverrideController["DEFAULT JUMPING"] = longGunAnimations[1];
                animatorOverrideController["DEFAULT SPRINTING"] = longGunAnimations[2];
                animatorOverrideController["DEFAULT WALKING"] = longGunAnimations[3];
                break;
            case 1:
                animatorOverrideController["DEFAULT IDLE"] = handGunAnimations[0];
                animatorOverrideController["DEFAULT JUMPING"] = handGunAnimations[1];
                animatorOverrideController["DEFAULT SPRINTING"] = handGunAnimations[2];
                animatorOverrideController["DEFAULT WALKING"] = handGunAnimations[3];
                break;
        }

    }

    public UnityEvent OnShootingEnabledToggle
    {
        get { return onShootingEnabledToggle; }
    }
}
