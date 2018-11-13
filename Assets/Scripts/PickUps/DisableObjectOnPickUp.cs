using UnityEngine;

public class DisableObjectOnPickUp : MonoBehaviour
{
    PickUpTheObject onPickUp;

    void Start()
    {
        Debug.Log(gameObject, gameObject);
        onPickUp = GetComponentInParent<PickUpTheObject>();
        onPickUp.OnPressed.AddListener(DisableMe);
    }

    void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
