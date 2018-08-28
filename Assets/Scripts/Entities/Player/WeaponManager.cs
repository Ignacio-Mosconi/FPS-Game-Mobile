﻿using System.Collections;
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
    WeaponShooting currentWeaponShooting;
    WeaponReloading currentWeaponReloading;
    WeaponType currentWeapon = WeaponType.LongGun;

	void Awake ()
    {
        SetEquippedWeapon();
	}
	
	void Update ()
    {
        if (InputManager.Instance.GetWeaponSwapAxis() > 0 && CanSwapWeapon())
            SwapWeapon(ScrollWheelDir.Up);
        if (InputManager.Instance.GetWeaponSwapAxis() < 0 && CanSwapWeapon())
            SwapWeapon(ScrollWheelDir.Down);
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

    bool CanSwapWeapon()
    {
        return (currentWeaponShooting.enabled && currentWeaponReloading.enabled);
    }

    public int GetCurrentWeaponIndex()
    {
        return (int)currentWeapon;
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