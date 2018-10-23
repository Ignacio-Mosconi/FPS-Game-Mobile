using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField] GameObject player;

	static PlayerManager instance;

	Life playerLife;
	PlayerMovement playerMovement;
	PlayerAnimation playerAnimation;
	PlayerAudio playerAudio;
	FirstPersonCamera firstPersonCamera;
	CharacterController characterController;
	WeaponManager weaponManager;

	void Awake()
	{
		if (Instance != this)
			Destroy(gameObject);
	}

	void Start()
	{
		playerLife = player.GetComponent<Life>();
		playerMovement = player.GetComponent<PlayerMovement>();
		playerAnimation = player.GetComponentInChildren<PlayerAnimation>();
		playerAudio = player.GetComponentInChildren<PlayerAudio>();
		firstPersonCamera = player.GetComponent<FirstPersonCamera>();
		characterController = player.GetComponent<CharacterController>();
		weaponManager = player.GetComponentInChildren<WeaponManager>();

		playerLife.OnDeath.AddListener(TogglePlayerAvailability);
	}

	void TogglePlayerAvailability()
	{
		playerMovement.enabled = !playerMovement.enabled;
		playerAnimation.enabled = !playerAnimation.enabled;
		playerAudio.enabled = !playerAudio.enabled;
		firstPersonCamera.enabled = !firstPersonCamera.enabled;
		characterController.enabled = !characterController.enabled;
		weaponManager.CurrentWeapon.enabled = !weaponManager.CurrentWeapon.enabled;
		weaponManager.enabled = !weaponManager.enabled;
	}

	PlayerManager Instance
	{
		get
		{
			if (!instance)
				instance = FindObjectOfType<PlayerManager>();
			if (!instance)
			{
				GameObject gameObj = new GameObject("Player Manager");
				instance = gameObj.AddComponent<PlayerManager>();
				player = FindObjectOfType<PlayerMovement>().gameObject;
			}

			return instance;
		}
	}
}
