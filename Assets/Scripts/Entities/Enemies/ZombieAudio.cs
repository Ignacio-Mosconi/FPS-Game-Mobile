using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ZombieAI))]
public class ZombieAudio : MonoBehaviour
{
	[SerializeField] AudioSource breathSound;
	[SerializeField] AudioSource chaseSound;
	[SerializeField] AudioSource attackSound;
	[SerializeField] AudioSource hitSound;
	ZombieAI zombieAI;
	Life zombieLife;

	void Awake()
	{
		zombieAI = GetComponent<ZombieAI>();
		zombieLife = GetComponent<Life>();
	}

	void Start()
	{
		zombieAI.OnChaseStart.AddListener(PlayChaseSound);
		zombieAI.OnChaseFinish.AddListener(PlayBreathSound);
		zombieAI.OnAttack.AddListener(PlayAttackSound);
		zombieLife.OnHit.AddListener(PlayHitSound);
		zombieLife.OnDeath.AddListener(DisableSelf);

		PlayBreathSound();
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

	void DisableSelf()
	{
		breathSound.Stop();
		chaseSound.Stop();
		attackSound.Stop();
		enabled = false;
	}
}
