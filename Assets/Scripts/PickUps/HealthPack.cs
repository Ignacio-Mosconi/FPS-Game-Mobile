using UnityEngine;

public class HealthPack :  Pickable
{
    [SerializeField] Life playerLife;
    [SerializeField] [Range(0f, 100f)] float healAmount;

    bool isPickingUpHealth = false;

    void DisableSelf()
    {
        gameObject.SetActive(false);
        Destroy(GetComponent("Pickable"));
    }

    override protected void PickUpObject()
    {
        if (!isPickingUpHealth)
        {
            isPickingUpHealth = true;

            playerLife.Heal(healAmount);
    
            iconCanvas.enabled = false;
            pickUpSound.Play();
            Invoke("DisableSelf", pickUpSound.clip.length);

            onInteractRange.Invoke(GetPickableName(), CanPickUp(), false);
            onPressed.Invoke();
        }
    }

    override protected string GetPickableName()
    {   
        return "Health Pack";
    }

    override protected bool CanPickUp()
    {
        return playerLife.Health < playerLife.MaxHealth;
    }

    override public PickUpType GetPickUpType()
    {
        return PickUpType.HealthPack;
    }
}