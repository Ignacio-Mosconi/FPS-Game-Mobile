using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(ZombieAttacking))]
public class ZombieAudio : MonoBehaviour
{
	[SerializeField] AudioSource breathSound;
	[SerializeField] AudioSource chaseSound;
	[SerializeField] AudioSource attackSound;
	[SerializeField] AudioSource hitSound;

	ZombieMovement zombieMovement;
	ZombieAttacking zombieAttacking;
	Life zombieLife;

	void Awake()
	{
		zombieMovement = GetComponent<ZombieMovement>();
		zombieAttacking = GetComponent<ZombieAttacking>();
		zombieLife = GetComponent<Life>();
	}

	void Start()
	{
		zombieMovement.OnChaseStart.AddListener(PlayChaseSound);
		zombieMovement.OnChaseFinish.AddListener(PlayBreathSound);
		zombieAttacking.OnAttack.AddListener(PlayAttackSound);
		zombieLife.OnHit.AddListener(PlayHitSound);
	}

	void PlayChaseSound()
	{
		if (breathSound.isPlaying)
			breathSound.Stop();
		chaseSound.Play();
	}

	void PlayBreathSound()
	{
		if (chaseSound.isPlaying)
			chaseSound.Stop();
		breathSound.Play();
	}

	void PlayAttackSound()
	{
		attackSound.Play();
	}

	void PlayHitSound()
	{
		hitSound.Play();
	}
}
