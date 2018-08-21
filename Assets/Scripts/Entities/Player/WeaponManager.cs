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
    PlayerShooting playerShooting;
    PlayerReloading playerReloading;
    WeaponType currentWeapon = WeaponType.HandGun;

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
            weapon.gameObject.SetActive(i == (int)currentWeapon);
            if (i == (int)currentWeapon)
            {
                playerShooting = weapon.gameObject.GetComponent<PlayerShooting>();
                playerReloading = weapon.gameObject.GetComponent<PlayerReloading>();
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

    public PlayerShooting PlayerShooting
    {
        get { return playerShooting; }
    }

    public PlayerReloading PlayerReloading
    {
        get { return playerReloading; }
    }

    public UnityEvent OnWeaponSwap
    {
        get { return onWeaponSwap; }
    }
}
