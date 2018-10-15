using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour 
{
    [Header("HUD Elements")]
    [SerializeField] Image crosshair;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI crateText;
    [SerializeField] GameObject mobileControls;
    [Header("References")]
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Life playerLife;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Transform cratesHolder;
    PickUpTheObject[] crates;
    const float criticalLifePercentage = 0.25f;
    const float criticalMagAmmoPercentage = 0.25f;
    float criticalLife = 0;
    int criticalMagAmmo = 0;
    int criticalAmmoLeft = 0;

    void Awake()
    {
        #if UNITY_STANDALONE        
            mobileControls.SetActive(false);
        #endif
    }
    
    void Start() 
	{
        ChangeAmmoDisplay();

        playerAnimation.OnShootingEnabledToggle.AddListener(CrosshairEnabledToggle);
        playerLife.OnHit.AddListener(ChangeHealthDisplay);
        weaponManager.OnWeaponSwap.AddListener(ChangeWeaponWeaponInfo);
        weaponManager.OnWeaponSwap.AddListener(ChangeAmmoDisplay);

        crates = cratesHolder.GetComponentsInChildren<PickUpTheObject>();

        foreach (PickUpTheObject crate in crates)
        {
            crate.OnDetected.AddListener(ChangeTextSituation);
            crate.OnPressed.AddListener(ChangeAmmoDisplay);
        }

        foreach (Transform weapon in weaponManager.transform)
        {
           weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(ChangeAmmoDisplay);
           weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(ChangeAmmoDisplay);
           weapon.gameObject.GetComponent<Weapon>().OnCrossHairScale.AddListener(ScaleCrosshair);
        }
        
        criticalLife = playerLife.MaxHealth * criticalLifePercentage;

        crateText.enabled = false;

        crateText.text = InputManager.Instance.CheckControllerConnection() ?
                        "Hold 'Square' to pick up ammo" : "Press 'E' to pick up ammo";

	}

    void CrosshairEnabledToggle()
    {
        crosshair.enabled = weaponManager.CurrentWeapon.enabled;
    }

    void ChangeAmmoDisplay()
    {
        string bulletsInMag = weaponManager.CurrentWeapon.BulletsInMag.ToString();
        string ammoLeft = weaponManager.CurrentWeapon.AmmoLeft.ToString();

        ammoText.text = bulletsInMag + "/" + ammoLeft;

        if (weaponManager.CurrentWeapon.BulletsInMag > criticalMagAmmo && weaponManager.CurrentWeapon.AmmoLeft > criticalAmmoLeft)
            ammoText.color = Color.white;
        else
        {
            if (weaponManager.CurrentWeapon.AmmoLeft > criticalAmmoLeft)
                ammoText.color = Color.yellow;
            else
                ammoText.color = Color.red;
        }
    }

    void ChangeHealthDisplay()
    {
        int hp = (int)playerLife.Health;
        string health = hp.ToString();

        healthText.text = health;
        healthText.color = (playerLife.Health > criticalLife) ? Color.white : Color.red;
    }

    void ChangeTextSituation()
    {
        crateText.enabled = !crateText.enabled;
    }

    void ChangeWeaponWeaponInfo()
    {
        criticalMagAmmo = (int)(weaponManager.CurrentWeapon.MagSize * criticalMagAmmoPercentage);
        criticalAmmoLeft = weaponManager.CurrentWeapon.MagSize;
    }

    void ScaleCrosshair()
    {
        float newScale = weaponManager.CurrentWeapon.CrosshairScaling;
        
        crosshair.transform.localScale = new Vector2(newScale, newScale);
    }
}
