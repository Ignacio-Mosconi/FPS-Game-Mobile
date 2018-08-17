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
    [SerializeField] PlayerShooting playerShooting;
    [SerializeField] PlayerReloading playerReloading;
    [SerializeField] Life playerLife;

    void Awake() 
	{
        playerAnimation.OnShootingEnabledToggle.AddListener(CrosshairEnabledToggle);
        playerReloading.OnAmmoChange.AddListener(ChangeAmmoDisplay);
        playerLife.OnHit.AddListener(ChangeHealthDisplay);
	}

    void CrosshairEnabledToggle()
    {
        crosshair.enabled = playerShooting.enabled;
    }

    void ChangeAmmoDisplay()
    {
        string bulletsInMag = playerReloading.BulletsInMag.ToString();
        string ammoLeft = playerReloading.AmmoLeft.ToString();

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
