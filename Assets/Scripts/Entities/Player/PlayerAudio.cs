using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour 
{
	[Header("Audio Sources")]
	[SerializeField] AudioSource[] indoorFootsteps;
	[SerializeField] AudioSource hitSound;
	[SerializeField] AudioSource deathSound;
	[SerializeField] AudioSource[] outdoorFootsteps;
	
	const float WALKING_VELOCITY = 0.25f;
	const float SPRINTING_VELOCITY = 0.6f;
	
	AudioSource[] currentFootsteps;
	Animator animator;
	PlayerMovement playerMovement;
	WeaponManager weaponManager;
	Life playerLife;

	void Awake()
	{
		animator = GetComponent<Animator>();
		playerMovement = GetComponentInParent<PlayerMovement>();
		weaponManager = GetComponentInChildren<WeaponManager>();
		playerLife = GetComponentInParent<Life>();
	}

	void Start()
	{
		foreach (Transform weapon in weaponManager.transform)
		{
			weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(PlayShootSound);
			weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(PlayReloadSound);
			weapon.gameObject.GetComponent<Weapon>().OnEmptyMag.AddListener(PlayEmptyMagSound);
		}
		
		ChangeFootstepsSounds();
		playerMovement.OnSurfaceChange.AddListener(ChangeFootstepsSounds);
		playerLife.OnHit.AddListener(PlayHitSound);
		playerLife.OnDeath.AddListener(PlayDeathSound);
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

        if (currentVelocity > WALKING_VELOCITY && currentVelocity <= SPRINTING_VELOCITY)
            currentFootsteps[stepNumber].Play();
    }

	void PlaySprintingSound(int stepNumber)
	{
		if (animator.GetFloat("Horizontal Velocity") > SPRINTING_VELOCITY)
			currentFootsteps[stepNumber].Play();
	}

	void PlayHitSound()
	{
		if (!hitSound.isPlaying)
			hitSound.Play();
	}

	void PlayDeathSound()
	{
		deathSound.Play();	
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
