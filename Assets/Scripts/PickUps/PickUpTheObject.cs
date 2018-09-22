using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpTheObject : MonoBehaviour
{
    Transform player;
    Transform firstPersonCamera;
    [SerializeField] float distanceToInteract;
    [SerializeField] LayerMask possibleTargets;
    [SerializeField] Weapon weaponTarget;
    [SerializeField] UnityEvent onDetected;
    [SerializeField] UnityEvent onPressed;
    AudioSource pickUpSound;
    static float holdInteractTime = 0;
    const float HOLD_INTERACT_TIME = 0.75f;
    bool isLooking = false;

    void Start()
    {
        pickUpSound = GetComponent<AudioSource>();
        player = GameObject.Find("Player").transform;
        firstPersonCamera = player.GetChild(0).transform;

        if(weaponTarget.name == "AK-47")
        {
            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshes.Length; i++)
                meshes[i].material.color = Color.cyan;
        }
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

                holdInteractTime = InputManager.Instance.GetInteractHoldButton() ? holdInteractTime + Time.deltaTime : 0;

                if (InputManager.Instance.GetInteractButton() || holdInteractTime > HOLD_INTERACT_TIME)
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
