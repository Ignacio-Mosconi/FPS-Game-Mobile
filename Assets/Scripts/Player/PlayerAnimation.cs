using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour 
{
    [SerializeField] UnityEvent onShootingEnabledToggle;
    //[SerializeField] AnimationClip reloadingAnimation;
    [SerializeField] WeaponManager weaponManager;
    Animator animator;
    CharacterController charController;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    //PlayerReloading playerReloading;

    void Awake()
    {
        animator = GetComponent<Animator>();
        //playerShooting = weaponHolder.GetComponentInChildren<PlayerShooting>();
        //playerReloading = weaponHolder.GetComponentInChildren<PlayerReloading>();
        charController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        weaponManager.PlayerShooting.OnShot.AddListener(HasShot);
        weaponManager.PlayerReloading.OnReload.AddListener(HasReloaded);
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

    public UnityEvent OnShootingEnabledToggle
    {
        get { return onShootingEnabledToggle; }
    }
}
