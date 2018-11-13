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
    [SerializeField] TextMeshProUGUI[] crateText;
    [SerializeField] GameObject crateButtonIconKeyboard;
    [SerializeField] GameObject crateButtonIconController;
    [SerializeField] Image crateButtonMobile;
    [SerializeField] GameObject mobileControls;
    
    [Header("References")]
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Life playerLife;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Transform cratesHolder;
    PickUpTheObject[] crates;
    
    const float CRITICAL_LIFE_PERC = 0.25f;
    const float CRITICAL_AMMO_PERC = 0.25f;
    
    float criticalLife = 0f;
    int criticalMagAmmo = 0;
    int criticalAmmoLeft = 0;
    bool inInteractionRange = false;

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
        
        criticalLife = playerLife.MaxHealth * CRITICAL_LIFE_PERC;

        crateText[0].text = InputManager.Instance.CheckControllerConnection() ? "Hold" : "Press";

        foreach (TextMeshProUGUI text in crateText)
            text.enabled = false;

        crateButtonIconKeyboard.SetActive(false);
        crateButtonIconController.SetActive(false);
#if UNITY_ANDROID
        crateButtonMobile.color = new Color(crateButtonMobile.color.r, crateButtonMobile.color.g, crateButtonMobile.color.b, 0f);
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

    void ChangeTextSituation()
    {
        inInteractionRange = !inInteractionRange;

        foreach (TextMeshProUGUI text in crateText)
            text.enabled = !text.enabled;
        
#if UNITY_STANDALONE
        bool controllerConnected = InputManager.Instance.CheckControllerConnection();
        
        if (controllerConnected)
            crateText[0].text = "Hold";
        else
            crateText[0].text = "Press";
        crateButtonIconController.SetActive(controllerConnected && inInteractionRange);
        crateButtonIconKeyboard.SetActive(!controllerConnected && inInteractionRange);
#else
        crateButtonMobile.color = crateText[0].enabled ? 
                                    new Color(crateButtonMobile.color.r, crateButtonMobile.color.g, crateButtonMobile.color.g, 200f) :
                                    new Color(crateButtonMobile.color.r, crateButtonMobile.color.g, crateButtonMobile.color.g, 0f);
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