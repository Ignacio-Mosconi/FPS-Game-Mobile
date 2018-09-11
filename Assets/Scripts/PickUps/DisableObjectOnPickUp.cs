using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectOnPickUp : MonoBehaviour
{
    PickUpTheObject onPickUp;

    void Start()
    {
        onPickUp = GetComponentInParent<PickUpTheObject>();
        onPickUp.OnPressed.AddListener(DisableMe);
    }

    void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
