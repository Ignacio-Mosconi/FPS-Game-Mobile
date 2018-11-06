using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum ScrollWheelDir
{
    Up, Down 
}

public class WeaponManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] UnityEvent onWeaponSwapStart;
    [SerializeField] UnityEvent onWeaponSwap;
    
    Weapon currentWeapon;
    Weapon.WeaponType currentWeaponType = Weapon.WeaponType.LongGun;  
    bool isSwapingWeapon = false;

	void Awake ()
    {
        SetEquippedWeapon();
	}
	
	void Update ()
    {
        if (CanSwapWeapon())
        {
            if (InputManager.Instance.GetWeaponSwapAxis() > 0)
            {
                StartCoroutine(SwapWeapon(ScrollWheelDir.Up));
                return;
            }
            if (InputManager.Instance.GetWeaponSwapAxis() < 0)
            {
                StartCoroutine(SwapWeapon(ScrollWheelDir.Down));
                return;
            }
        }
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

    IEnumerator SwapWeapon(ScrollWheelDir dir)
    {
        Weapon.WeaponType previousWeaponType = currentWeaponType;

        if (dir == ScrollWheelDir.Up)
        {
            if ((int)currentWeaponType < transform.childCount - 1)
                currentWeaponType++;
            else
                currentWeaponType = 0;
        }
        else
        {
            if ((int)currentWeaponType > 0)
                currentWeaponType--;
            else
                currentWeaponType = Weapon.WeaponType.Count - 1;
        }

        if (currentWeaponType != previousWeaponType)
        {
            isSwapingWeapon = true;
            onWeaponSwapStart.Invoke();
            yield return new WaitForSeconds(currentWeapon.SwapWeaponOutAnimation.length);
            SetEquippedWeapon();
            yield return new WaitForSeconds(currentWeapon.SwapWeaponInAnimation.length);
            isSwapingWeapon = false;
        }
    }

    bool CanSwapWeapon()
    {
        return (currentWeapon.enabled && !isSwapingWeapon);
    }

    public Weapon CurrentWeapon
    {
        get { return currentWeapon; }
    }

    public Weapon.WeaponType CurrentWeaponType
    {
        get { return currentWeaponType; }
    }

    public UnityEvent OnWeaponSwapStart
    {
        get { return onWeaponSwapStart; }
    }

    public UnityEvent OnWeaponSwap
    {
        get { return onWeaponSwap; }
    }
}