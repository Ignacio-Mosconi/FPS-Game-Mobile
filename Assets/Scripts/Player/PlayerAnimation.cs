using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerShooting))]

public class PlayerAnimation : MonoBehaviour 
{
    [SerializeField] UnityEvent onShootingEnabledToggle;
    [SerializeField] AnimationClip reloadingAnimation;
    Animator animator;
    CharacterController charController;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    PlayerReloading playerReloading;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerShooting = GetComponent<PlayerShooting>();
        playerReloading = GetComponent<PlayerReloading>();
        charController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        playerShooting.OnShot.AddListener(HasShot);
        playerReloading.OnReload.AddListener(HasReloaded);
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
                if (!playerShooting.enabled)
                    EnableShooting();
                if (!playerReloading.enabled)
                    EnableReloading();
            }
            else
            {
                if (normalizedVelocity >= 0.6)
                {
                    if (playerShooting.enabled)
                        DisableShooting();
                    if (playerReloading.enabled)
                        DisableReloading();
                }
            }
        }
        else
        {
            if (playerShooting.enabled)
                DisableShooting();
            if (playerReloading.enabled)
                DisableReloading();
        }

        animator.SetFloat("Horizontal Velocity", normalizedVelocity, 0.2f, Time.deltaTime);
        animator.SetFloat("Vertical Velocity", verticalVelocity, 0.2f, Time.deltaTime);
        animator.SetBool("Is Jumping", jumping);
	}

    void HasShot()
    {
        animator.SetBool("Is Shooting", true);
        Invoke("IsNotShooting", 1 / playerShooting.FireRate);
    }

    void IsNotShooting()
    {
        animator.SetBool("Is Shooting", false);
    }

    void HasReloaded()
    {
        animator.SetBool("Is Reloading", true);
        Invoke("IsNotReloading", reloadingAnimation.length);
    }

    void IsNotReloading()
    {
        animator.SetBool("Is Reloading", false);
    }

    void DisableShooting()
    {
        playerShooting.enabled = false;
        onShootingEnabledToggle.Invoke();
    }

    void EnableShooting()
    {
        playerShooting.enabled = true;
        onShootingEnabledToggle.Invoke();
    }

    void DisableReloading()
    {
        playerReloading.enabled = false;
    }

    void EnableReloading()
    {
        playerReloading.enabled = true;
    }

    public UnityEvent OnShootingEnabledToggle
    {
        get { return onShootingEnabledToggle; }
    }
}
