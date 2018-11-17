﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour 
{
    [Header("HUD Elements")]
    [SerializeField] Image crosshair;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI[] pickUpText;
    [SerializeField] TextMeshProUGUI cannotPickUpText;
    [SerializeField] GameObject crateButtonIconKeyboard;
    [SerializeField] GameObject crateButtonIconController;
    [SerializeField] Image crateButtonMobile;
    [SerializeField] GameObject mobileControls;
    
    [Header("References")]
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Life playerLife;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Transform cratesHolder;
    [SerializeField] Transform helathPacksHolder;
    
    const float CRITICAL_LIFE_PERC = 0.25f;
    const float CRITICAL_AMMO_PERC = 0.25f;
    
    List<Pickable> pickables;
#if UNITY_ANDROID
    Color enabledColor;
    Color disabledColor;
#endif
    float criticalLife = 0f;
    int criticalMagAmmo = 0;
    int criticalAmmoLeft = 0;

    void Awake()
    {
#if UNITY_STANDALONE        
        mobileControls.SetActive(false);
#else
        enabledColor = new Color(crateButtonMobile.color.r, crateButtonMobile.color.g, crateButtonMobile.color.g, 200f);
        disabledColor = new Color(crateButtonMobile.color.r, crateButtonMobile.color.g, crateButtonMobile.color.g, 0f);
#endif
    }
    
    void Start() 
	{
        ChangeAmmoDisplay();

        playerAnimation.OnShootingEnabledToggle.AddListener(CrosshairEnabledToggle);
        playerLife.OnHit.AddListener(ChangeHealthDisplay);
        weaponManager.OnWeaponSwap.AddListener(ChangeWeaponWeaponInfo);
        weaponManager.OnWeaponSwap.AddListener(ChangeAmmoDisplay);

        pickables = new List<Pickable>();

        foreach (Transform crate in cratesHolder)
            pickables.Add(crate.GetComponent<Pickable>());

        foreach (Pickable pickable in pickables)
        {
            pickable.OnInteractRange.AddListener(ChangeTextSituation);
            pickable.OnPressed.AddListener(ChangeAmmoDisplay);
        }

        foreach (Transform weapon in weaponManager.transform)
        {
           weapon.gameObject.GetComponent<Weapon>().OnShot.AddListener(ChangeAmmoDisplay);
           weapon.gameObject.GetComponent<Weapon>().OnReload.AddListener(ChangeAmmoDisplay);
           weapon.gameObject.GetComponent<Weapon>().OnCrossHairScale.AddListener(ScaleCrosshair);
        }
        
        criticalLife = playerLife.MaxHealth * CRITICAL_LIFE_PERC;

        pickUpText[0].text = InputManager.Instance.CheckControllerConnection() ? "Hold" : "Press";

        foreach (TextMeshProUGUI text in pickUpText)
            text.enabled = false;
        cannotPickUpText.enabled = false;

        crateButtonIconKeyboard.SetActive(false);
        crateButtonIconController.SetActive(false);
#if UNITY_ANDROID
        crateButtonMobile.color = disabledColor;
#endif
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

    void ChangeTextSituation(string pickableName, bool canPickUp, bool inRange)
    {
        foreach (TextMeshProUGUI text in pickUpText)
            text.enabled = canPickUp && inRange;
        cannotPickUpText.enabled = !canPickUp && inRange;

        if (inRange)
        {
            if (canPickUp)
                pickUpText[1].text = "to pick up the " + pickableName + ".";
            else
                cannotPickUpText.text = "You cannot pick up more " + pickableName + ".";
        }
        
#if UNITY_STANDALONE
        bool controllerConnected = InputManager.Instance.CheckControllerConnection();
        
        if (inRange && canPickUp)
        {
            if (controllerConnected)
                pickUpText[0].text = "Hold";
            else
                pickUpText[0].text = "Press";
        }
        crateButtonIconController.SetActive(controllerConnected && inRange && canPickUp);
        crateButtonIconKeyboard.SetActive(!controllerConnected && inRange && canPickUp);
#else
        crateButtonMobile.color = inRange ? enabledColor : disabledColor;
#endif
    }

    void ChangeWeaponWeaponInfo()
    {
        criticalMagAmmo = (int)(weaponManager.CurrentWeapon.MagSize * CRITICAL_AMMO_PERC);
        criticalAmmoLeft = weaponManager.CurrentWeapon.MagSize;
    }

    void ScaleCrosshair()
    {
        float newScale = weaponManager.CurrentWeapon.CrosshairScaling;
        
        crosshair.transform.localScale = new Vector2(newScale, newScale);
    }
}