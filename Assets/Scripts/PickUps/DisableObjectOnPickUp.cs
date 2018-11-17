using UnityEngine;

public class DisableObjectOnPickUp : MonoBehaviour
{
    Pickable onPickUp;

    void Start()
    {
        Debug.Log(gameObject, gameObject);
        onPickUp = GetComponentInParent<Pickable>();
        onPickUp.OnPressed.AddListener(DisableMe);
    }

    void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
