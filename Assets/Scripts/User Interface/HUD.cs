using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour 
{
    [SerializeField] Image crosshair;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Life playerLife;
    [SerializeField] WeaponManager weaponManager;

    void Start() 
	{
        playerAnimation.OnShootingEnabledToggle.AddListener(CrosshairEnabledToggle);
        playerLife.OnHit.AddListener(ChangeHealthDisplay);
        weaponManager.OnWeaponSwap.AddListener(ChangeAmmoDisplay);

        for (int i = 0; i < weaponManager.GetNumberOfWeapons(); i++)
            weaponManager.GetWeaponReloadingAtIndex(i).OnAmmoChange.AddListener(ChangeAmmoDisplay);
	}

    void CrosshairEnabledToggle()
    {
        crosshair.enabled = weaponManager.CurrentWeaponShooting.enabled;
    }

    void ChangeAmmoDisplay()
    {
        string bulletsInMag = weaponManager.CurrentWeaponReloading.BulletsInMag.ToString();
        string ammoLeft = weaponManager.CurrentWeaponReloading.AmmoLeft.ToString();

        ammoText.text = bulletsInMag + "/" + ammoLeft;
    }

    void ChangeHealthDisplay()
    {
        int hp = (int)playerLife.Health;
        string health = hp.ToString();

        healthText.text = health;
        healthText.color = (playerLife.Health > 25) ? Color.white : Color.red;
    }
}
