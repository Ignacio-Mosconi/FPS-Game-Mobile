using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : Pickable
{
    [SerializeField] [Range(0, 10)] int magsToCollect;
    [SerializeField] Weapon weaponTarget;
	
    protected override void Start()
    {
        base.Start();

        if (weaponTarget.name == "AK-47")
        {
            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshes.Length; i++)
                meshes[i].material.color = Color.yellow;
        }
    }

    override protected void PickUpObject()
    {
        weaponTarget.AmmoLeft += magsToCollect * weaponTarget.MagSize;
        iconCanvas.enabled = false;
        
        animator.SetTrigger("Was Picked Up");
        pickUpSound.Play();

        onInteractRange.Invoke(GetPickableName(), CanPickUp(), false);
        onPressed.Invoke();

        Destroy(GetComponent("Pickable"));
    }

    override protected string GetPickableName()
    {
        string weaponTypeName = "";

        switch (weaponTarget.TypeOfWeapon)
        {
            case Weapon.WeaponType.HandGun:
                weaponTypeName = "Handgun";
                break;
            case Weapon.WeaponType.LongGun:
                weaponTypeName = "Long Gun";
                break;
            default:
                break;
        }
        
        return weaponTypeName + " Ammo";
    }

    override protected bool CanPickUp()
    {
        return weaponTarget.AmmoLeft < weaponTarget.MaxAmmo;
    }

    override public PickUpType GetPickUpType()
    {
        return PickUpType.AmmoCrate;
    }
}
