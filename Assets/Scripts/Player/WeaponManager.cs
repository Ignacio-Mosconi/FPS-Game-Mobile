using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ScrollWheelDir
{
    Up, Down 
}

public class WeaponManager : MonoBehaviour
{
    PlayerShooting playerShooting;
    PlayerReloading playerReloading;
    int currentWeapon = 0;

	void Awake ()
    {
        SetEquippedWeapon();
	}
	
	void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            SwapWeapon(ScrollWheelDir.Up);
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            SwapWeapon(ScrollWheelDir.Down);
	}

    void SetEquippedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == currentWeapon);
            if (i == currentWeapon)
            {
                playerShooting = weapon.GetComponent<PlayerShooting>();
                playerReloading = weapon.GetComponent<PlayerReloading>();
            }
            i++;
        }
    }

    void SwapWeapon(ScrollWheelDir dir)
    {
        int previousWeapon = currentWeapon;

        if (dir == ScrollWheelDir.Up)
        {
            if (currentWeapon < transform.childCount - 1)
                currentWeapon++;
            else
                currentWeapon = 0;
        }
        else
        {
            if (currentWeapon > 0)
                currentWeapon--;
            else
                currentWeapon = 0;
        }

        if (currentWeapon != previousWeapon)
            SetEquippedWeapon();
    }

    public PlayerShooting PlayerShooting
    {
        get { return playerShooting; }
    }

    public PlayerReloading PlayerReloading
    {
        get { return playerReloading; }
    }
}
