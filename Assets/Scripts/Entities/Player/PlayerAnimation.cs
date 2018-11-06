using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    
    [Header("Animations")]
    [SerializeField] AnimationClip[] longGunAnimations;
    [SerializeField] AnimationClip[] handGunAnimations;
    
    [Header("Events")]
    [SerializeField] UnityEvent onShootingEnabledToggle;
    
    const float ANIMATION_DAMP_TIME = 0.2f;
    const float SPRINTING_VELOCITY = 0.6f;
    
    Animator animator;
    CharacterController charController;
    PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        charController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        weaponManager.OnWeaponSwapStart.AddListener(HasStartedToSwapWeapn);
        weaponManager.OnWeaponSwap.AddListener(ChangeWeaponAnimations);
        foreach (Transform weapon in weaponManager.transform)
        {
            weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(HasShot);
            weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(HasReloaded);
        }

        animator.runtimeAnimatorController = animatorOverrideController;

        ChangeWeaponAnimations();
    }

    void Update() 
	{
        Vector3 horVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);
        float normalizedVelocity = horVelocity.magnitude / playerMovement.MovementSpeed;
        float verticalVelocity = charController.velocity.y;
        bool jumping = playerMovement.IsJumping();

        if (!jumping && !weaponManager.CurrentWeapon.IsReloading && normalizedVelocity < SPRINTING_VELOCITY)
        {
            if (!weaponManager.CurrentWeapon.enabled)
                EnableShooting();
        }
        else
        {
            if (weaponManager.CurrentWeapon.enabled)
                DisableShooting();
        }

        animator.SetFloat("Horizontal Velocity", normalizedVelocity,ANIMATION_DAMP_TIME, Time.deltaTime);
        animator.SetFloat("Vertical Velocity", verticalVelocity, ANIMATION_DAMP_TIME, Time.deltaTime);
        animator.SetBool("Is Jumping", jumping);
	}

    void HasShot()
    {
        animator.SetTrigger("Has Shot");
    }

    void HasReloaded()
    {
        animator.SetTrigger("Has Reloaded");
    }

    void HasStartedToSwapWeapn()
    {
        animator.SetTrigger("Has Swapped Weapon");
    }

    void EnableShooting()
    {
        weaponManager.CurrentWeapon.enabled = true;
        onShootingEnabledToggle.Invoke();
    }

    void DisableShooting()
    {
        weaponManager.CurrentWeapon.enabled = false;
        onShootingEnabledToggle.Invoke();
    }

    void ChangeWeaponAnimations()
    {
        animatorOverrideController["DEFAULT SHOOTING"] = weaponManager.CurrentWeapon.ShootAnimation;
        animatorOverrideController["DEFAULT RELOADING"] = weaponManager.CurrentWeapon.ReloadAnimation;

        switch (weaponManager.CurrentWeaponType)
        {
            case Weapon.WeaponType.LongGun:
                animatorOverrideController["DEFAULT IDLE"] = longGunAnimations[0];
                animatorOverrideController["DEFAULT JUMPING"] = longGunAnimations[1];
                animatorOverrideController["DEFAULT SPRINTING"] = longGunAnimations[2];
                animatorOverrideController["DEFAULT WALKING"] = longGunAnimations[3];
                animatorOverrideController["DEFAULT SWAPPING WEAPON IN"] = longGunAnimations[4];
                animatorOverrideController["DEFAULT SWAPPING WEAPON OUT"] = longGunAnimations[5];
                break;
            case Weapon.WeaponType.HandGun:
                animatorOverrideController["DEFAULT IDLE"] = handGunAnimations[0];
                animatorOverrideController["DEFAULT JUMPING"] = handGunAnimations[1];
                animatorOverrideController["DEFAULT SPRINTING"] = handGunAnimations[2];
                animatorOverrideController["DEFAULT WALKING"] = handGunAnimations[3];
                animatorOverrideController["DEFAULT SWAPPING WEAPON IN"] = handGunAnimations[4];
                animatorOverrideController["DEFAULT SWAPPING WEAPON OUT"] = handGunAnimations[5];
                break;
        }
    }

    public UnityEvent OnShootingEnabledToggle
    {
        get { return onShootingEnabledToggle; }
    }
}
