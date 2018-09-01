using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour 
{
	[SerializeField] AudioSource[] indoorFootsteps;
	[SerializeField] AudioSource[] outdoorFootsteps;
	AudioSource[] currentFootsteps;
	Animator animator;
	PlayerMovement playerMovement;
	WeaponManager weaponManager;

	void Awake()
	{
		animator = GetComponent<Animator>();
		playerMovement = GetComponentInParent<PlayerMovement>();
		weaponManager = GetComponentInChildren<WeaponManager>();
		foreach (Transform weapon in weaponManager.transform)
		{
			weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(PlayShootSound);
			weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(PlayReloadSound);
			weapon.gameObject.GetComponent<Weapon>().OnEmptyMag.AddListener(PlayEmptyMagSound);
		}
	}

	void Start()
	{
		ChangeFootstepsSounds();
		playerMovement.OnSurfaceChange.AddListener(ChangeFootstepsSounds);
	}

	void PlayShootSound()
	{
		weaponManager.CurrentWeapon.ShootSound.Play();
	}

	void PlayReloadSound()
	{
		weaponManager.CurrentWeapon.ReloadSound.Play();
	}

	void PlayEmptyMagSound()
	{
		if (!weaponManager.CurrentWeapon.EmptyMagSound.isPlaying)
			weaponManager.CurrentWeapon.EmptyMagSound.Play();
	}

	void PlayWalkingSound(int stepNumber)
	{
		float currentVelocity = animator.GetFloat("Horizontal Velocity");

        if (currentVelocity > 0.25 && currentVelocity <= 0.6)
            currentFootsteps[stepNumber].Play();
    }

	void PlaySprintingSound(int stepNumber)
	{
		if (animator.GetFloat("Horizontal Velocity") > 0.6)
			currentFootsteps[stepNumber].Play();
	}

	void ChangeFootstepsSounds()
	{
		switch (playerMovement.GetCurrentSurfaceIndex())
		{
			case (int)WalkingSurface.Indoors:
				currentFootsteps = indoorFootsteps;
				break;
			case (int)WalkingSurface.Outdoors:
				currentFootsteps = outdoorFootsteps;
				break;
		}
	}
}
