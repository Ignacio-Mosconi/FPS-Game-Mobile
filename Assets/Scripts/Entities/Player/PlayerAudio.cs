using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour 
{
	WeaponManager weaponManager;

	void Awake()
	{
		weaponManager = GetComponentInChildren<WeaponManager>();
		foreach (Transform weapon in weaponManager.transform)
		{
			weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(PlayShootSound);
			weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(PlayReloadSound);
			weapon.gameObject.GetComponent<Weapon>().OnEmptyMag.AddListener(PlayEmptyMagSound);
		}
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
}
