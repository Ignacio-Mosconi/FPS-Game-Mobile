using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpTheObject : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform firstPersonCamera;
    [SerializeField] float distanceToInteract;
    [SerializeField] LayerMask possibleTargets;
    [SerializeField] Weapon weaponTarget;
    [SerializeField] UnityEvent onDetected;
    [SerializeField] UnityEvent onPressed;

    AudioSource pickUpSound;

    bool isLooking = false;

    void Start()
    {
        pickUpSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 dist = player.position - transform.position;

        if (dist.magnitude < distanceToInteract)
        {
            RaycastHit hit;
            if (Physics.Raycast(firstPersonCamera.position, firstPersonCamera.forward, out hit, distanceToInteract, possibleTargets))
            {
                if (!isLooking)
                {
                    isLooking = true;
                    onDetected.Invoke();
                }

                if (Input.GetButtonDown("Interact"))
                {

                    weaponTarget.AmmoLeft = weaponTarget.MagSize;
                    
                    pickUpSound.Play();

                    onDetected.Invoke();
                    onPressed.Invoke();

                    

                    Destroy(GetComponent("PickUpTheObject"));
                }
            }
            else if (isLooking)
            {
                isLooking = false;
                onDetected.Invoke();
            }
                
        }
        else if (isLooking)
        {
            isLooking = false;
            onDetected.Invoke();
        }
    }

    public UnityEvent OnDetected
    {
        get { return onDetected; }
    }

    public UnityEvent OnPressed
    {
        get { return onPressed; }
    }
}
