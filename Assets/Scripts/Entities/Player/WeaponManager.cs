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
    Weapon currentWeapon;
    WeaponType currentWeaponType = WeaponType.LongGun;

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
            weapon.gameObject.SetActive(i == (int)currentWeaponType);
            if (i == (int)currentWeaponType)
            {
                currentWeapon = weapon.gameObject.GetComponent<Weapon>();
                onWeaponSwap.Invoke();
            }
            i++;
        }
    }

    void SwapWeapon(ScrollWheelDir dir)
    {
        WeaponType previousWeaponType = currentWeaponType;

        if (dir == ScrollWheelDir.Up)
        {
            if ((int)currentWeaponType < transform.childCount - 1)
                currentWeaponType++;
            else
                currentWeaponType = WeaponType.LongGun;
        }
        else
        {
            if ((int)currentWeaponType > 0)
                currentWeaponType--;
            else
                currentWeaponType = WeaponType.HandGun;
        }

        if (currentWeaponType != previousWeaponType)
            SetEquippedWeapon();
    }

    bool CanSwapWeapon()
    {
        return (currentWeapon.enabled);
    }

    public int GetCurrentWeaponIndex()
    {
        return (int)currentWeaponType;
    }

    public Weapon CurrentWeapon
    {
        get { return currentWeapon; }
    }

    public UnityEvent OnWeaponSwap
    {
        get { return onWeaponSwap; }
    }
}