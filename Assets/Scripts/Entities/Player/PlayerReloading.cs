using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerReloading : MonoBehaviour
{
    [SerializeField] int magSize;
    [SerializeField] int ammoLeft;
    [SerializeField] UnityEvent onAmmoChange;
    [SerializeField] UnityEvent onReload;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AnimationClip reloadAnimation;
    int bulletsInMag;

    void Start()
    {
        bulletsInMag = magSize;
        onAmmoChange.Invoke();
    }

    void Update()
    {
        if (Input.GetButtonDown("Reload") && bulletsInMag < magSize + 1)
            Reload();
    }

    void Reload()
    {
        if (ammoLeft > 0)
        {
            reloadSound.Play();
            if (ammoLeft > magSize)
            {
                if (bulletsInMag > 0)
                    bulletsInMag = magSize + 1;
                else
                    bulletsInMag = magSize;
                ammoLeft -= magSize;
            }
            else
            {
                bulletsInMag = ammoLeft;
                ammoLeft = 0;
            }
            onAmmoChange.Invoke();
            onReload.Invoke();
        }
    }

    public int BulletsInMag
    {
        get { return bulletsInMag; }
        set
        {
            bulletsInMag = value;
            onAmmoChange.Invoke();
        }
    }

    public int AmmoLeft
    {
        get { return ammoLeft; }
    }

    public UnityEvent OnAmmoChange
    {
        get { return onAmmoChange; }
    }

    public UnityEvent OnReload
    {
        get { return onReload; }
    }

    public float ReloadTime
    {
        get { return reloadAnimation.length; }
    }
}
