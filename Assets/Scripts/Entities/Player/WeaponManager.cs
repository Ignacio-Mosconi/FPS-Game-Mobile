using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum ScrollWheelDir
{
    Up, Down 
}

enum WeaponType
{
    LongGun, HandGun
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UnityEvent onWeaponSwap;
    WeaponShooting[2] weaponShootings;
    WeaponReloading[2] weaponReloadings;
    WeaponShooting currentWeaponShooting;
    WeaponReloading currentWeaponReloading;
    WeaponType currentWeapon = WeaponType.LongGun;

	void Awake ()
    {
        SetWeaponsArray();
        SetEquippedWeapon();
	}
	
	void Update ()
    {
        if (currentWeaponShooting.enabled && currentWeaponReloading.enabled && Input.GetAxis("Mouse ScrollWheel") > 0)
            SwapWeapon(ScrollWheelDir.Up);
        if (currentWeaponShooting.enabled && currentWeaponReloading.enabled && Input.GetAxis("Mouse ScrollWheel") < 0)
            SwapWeapon(ScrollWheelDir.Down);
	}

    void SetWeaponsArray()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weaponShootings[i] = weapon.gameObject.GetComponent<WeaponShooting>();
            weaponReloadings[i] = weapon.gameObject.GetComponent<WeaponReloading>();
            i++;
        }
    }

    void SetEquippedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == (int)currentWeapon);
            if (i == (int)currentWeapon)
            {
                currentWeaponShooting = weapon.gameObject.GetComponent<WeaponShooting>();
                currentWeaponReloading = weapon.gameObject.GetComponent<WeaponReloading>();
                onWeaponSwap.Invoke();
            }
            i++;
        }
    }

    void SwapWeapon(ScrollWheelDir dir)
    {
        WeaponType previousWeapon = currentWeapon;

        if (dir == ScrollWheelDir.Up)
        {
            if ((int)currentWeapon < transform.childCount - 1)
                currentWeapon++;
            else
                currentWeapon = WeaponType.LongGun;
        }
        else
        {
            if ((int)currentWeapon > 0)
                currentWeapon--;
            else
                currentWeapon = WeaponType.HandGun;
        }

        if (currentWeapon != previousWeapon)
            SetEquippedWeapon();
    }

    public int GetCurrentWeaponIndex()
    {
        return (int)currentWeapon;
    }

    public int GetNumberOfWeapons()
    {
        return transform.childCount;
    }

    public WeaponShooting GetWeaponShootingAtIndex(int i)
    {
        return weaponShootings[i];
    }

    public WeaponReloading GetWeaponReloadingAtIndex(int i)
    {
        return weaponReloadings[i];
    }

    public WeaponShooting CurrentWeaponShooting
    {
        get { return currentWeaponShooting; }
    }

    public WeaponReloading CurrentWeaponReloading
    {
        get { return currentWeaponReloading; }
    }

    public UnityEvent OnWeaponSwap
    {
        get { return onWeaponSwap; }
    }
}
