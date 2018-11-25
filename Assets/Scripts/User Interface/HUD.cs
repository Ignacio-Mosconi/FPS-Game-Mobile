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
    [SerializeField] TextMeshProUGUI[] pickUpText;
    [SerializeField] TextMeshProUGUI[] cannotPickUpText;
    [SerializeField] GameObject crateButtonIconKeyboard;
    [SerializeField] GameObject crateButtonIconController;
    [SerializeField] Image crateButtonMobile;
    [SerializeField] GameObject mobileControls;
    [SerializeField] Transform[] zombiesTextTransform;

    [Header("References")]
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Life playerLife;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Transform cratesHolder;
    [SerializeField] Transform healthPacksHolder;
    [SerializeField] List<Transform> enemyGroups;
    [SerializeField] List<NotifyAtTrigger> zombieZones;

    const float CRITICAL_LIFE_PERC = 0.25f;
    const float CRITICAL_AMMO_PERC = 0.25f;
    const float MIN_TRANSPARENCY   = 0.40f;

    List<Pickable> pickables;
#if UNITY_ANDROID
    Color enabledColor;
    Color disabledColor;
#endif
    float criticalLife = 0f;
    int criticalMagAmmo = 0;
    int criticalAmmoLeft = 0;
    List<TextMeshProUGUI> enemyGroupText;
    List<int> maxZombies;

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
        foreach (Transform healthPack in healthPacksHolder)
            pickables.Add(healthPack.GetComponent<Pickable>());

        foreach (Pickable pickable in pickables)
        {
            pickable.OnInteractRange.AddListener(ChangeTextSituation);
            if (pickable.GetPickUpType() == Pickable.PickUpType.AmmoCrate)
                pickable.OnPressed.AddListener(ChangeAmmoDisplay);
            if (pickable.GetPickUpType() == Pickable.PickUpType.HealthPack)
                pickable.OnPressed.AddListener(ChangeHealthDisplay);
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
        foreach (TextMeshProUGUI text in cannotPickUpText)
            text.enabled = false;

        crateButtonIconKeyboard.SetActive(false);
        crateButtonIconController.SetActive(false);
#if UNITY_ANDROID
        crateButtonMobile.color = disabledColor;
#endif

        // Zombies texts
        enemyGroupText = new List<TextMeshProUGUI>();
        maxZombies = new List<int>();

        for (int i = 0; i < enemyGroups.Count; i++)
        {
            maxZombies.Add(0);
            for (int j = 0; j < enemyGroups[i].childCount; j++)
            {
                Transform child = enemyGroups[i].GetChild(j);

                if (child.gameObject.layer == LayerMask.NameToLayer("Zombies Main"))
                {
                    Life life = child.GetComponent<Life>();

                    if (!life) Debug.Log("No life component at " + life.name);

                    life.OnDeath.AddListener(UpdateZombieTextValues);

                    maxZombies[i]++;
                }
            }

            enemyGroupText.Add(zombiesTextTransform[i].GetComponentInChildren<TextMeshProUGUI>());
            int group = i + 1;
            enemyGroupText[i].text = "G" + group + ": " + maxZombies[i] + "/" + maxZombies[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < zombieZones.Count; i++)
        {
            Vector3 localScale = zombiesTextTransform[i].localScale;
            
            if (zombieZones[i].IsTriggering)
            {
                if (localScale.x < 1.25f)
                {
                    float scale = Mathf.Lerp(localScale.x, 1.25f, 3.0f);

                    localScale.Set(
                        scale, // X
                        scale, // y
                        1.0f   // z
                        );

                    zombiesTextTransform[i].localScale = localScale;
                }
            }
            else
            {
                if (localScale.x > 1.0f)
                {
                    float scale = Mathf.Lerp(localScale.x, 1.0f, 3.0f);

                    localScale.Set(
                        scale, // X
                        scale, // y
                        1.0f   // z
                        );

                    zombiesTextTransform[i].localScale = localScale;
                }
            }
        }
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
        foreach (TextMeshProUGUI text in cannotPickUpText)
            text.enabled = !canPickUp && inRange;

        if (inRange)
        {
            if (canPickUp)
            {
                pickUpText[1].text = "to pick up the";
                pickUpText[2].text = pickableName;
            }
            else
            {
                cannotPickUpText[0].text = "You cannot pick up more ";
                cannotPickUpText[1].text = pickableName;
            }
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
        crateButtonMobile.color = inRange && canPickUp ? enabledColor : disabledColor;
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

    void UpdateZombieTextValues()
    {
        for (int i = 0; i < enemyGroups.Count; i++)
        {
            float countOfZombies = 0;
            for (int j = 0; j < enemyGroups[i].childCount; j++)
                if (enemyGroups[i].GetChild(j).gameObject.layer == LayerMask.NameToLayer("Zombies Main"))
                    countOfZombies++;

            float percentage = countOfZombies / maxZombies[i];

            if (percentage < 0.4f)
            {
                if (percentage > 0.2f)
                    enemyGroupText[i].color = new Color(1.0f, 0.92f, 0.016f, enemyGroupText[i].color.a); // Yellow
                else if (percentage > 0.0f)
                    enemyGroupText[i].color = new Color(1.0f, 0.0f,  0.0f,   enemyGroupText[i].color.a); // Red
                else
                    enemyGroupText[i].color = new Color(0.3f, 0.3f,  0.3f,   enemyGroupText[i].color.a); // Black Grey
            }

            int group = i + 1;
            enemyGroupText[i].text = "G" + group + ": " + countOfZombies + "/" + maxZombies[i];
        }
    }
}