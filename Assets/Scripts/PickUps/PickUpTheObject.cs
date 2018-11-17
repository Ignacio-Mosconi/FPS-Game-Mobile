using UnityEngine;
using UnityEngine.Events;

public class PickUpTheObject : MonoBehaviour
{
    [SerializeField] float distanceToInteract;
    [SerializeField] LayerMask possibleTargets;
    [SerializeField] Weapon weaponTarget;
    
    Transform player;
    Transform firstPersonCamera;
    AudioSource pickUpSound;
    Canvas iconCanvas; 
    
    static float holdInteractTime = 0;
    
    const float HOLD_INTERACT_TIME = 0.75f;
    
    bool isLooking = false;

    UnityEvent onDetected = new UnityEvent();
    UnityEvent onPressed = new UnityEvent();
    
    void Start()
    {
        pickUpSound = GetComponent<AudioSource>();
        iconCanvas = GetComponentInChildren<Canvas>();
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

                holdInteractTime = InputManager.Instance.GetInteractHoldButton() ? holdInteractTime + Time.deltaTime : 0f;

                if ((InputManager.Instance.GetInteractButton() || holdInteractTime > HOLD_INTERACT_TIME) &&
                    weaponTarget.AmmoLeft < weaponTarget.MaxAmmo)
                {
                    weaponTarget.AmmoLeft += weaponTarget.MagSize;
                    
                    pickUpSound.Play();
                    iconCanvas.enabled = false;

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