using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractRangeEvent : UnityEvent<string, bool, bool> {}

public abstract class Pickable : MonoBehaviour
{
    public enum PickUpType
    {
        AmmoCrate, HealthPack
    }

    [SerializeField] float distanceToInteract;
    
    int layerMask;
    Transform player;
    Transform firstPersonCamera;
    
    static float holdInteractTime = 0;
    
    const float HOLD_INTERACT_TIME = 0.75f;
    
    bool isLooking = false;
    
    protected Canvas iconCanvas;
    protected Animator animator;
    protected AudioSource pickUpSound;
    
    protected InteractRangeEvent onInteractRange = new InteractRangeEvent();
    protected UnityEvent onPressed = new UnityEvent();
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        pickUpSound = GetComponent<AudioSource>();
        iconCanvas = GetComponentInChildren<Canvas>();
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
    }

    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        firstPersonCamera = player.GetChild(0).transform;
    }

    void Update()
    {
        Vector3 dist = player.position - transform.position;

        if (dist.magnitude < distanceToInteract)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(firstPersonCamera.position, firstPersonCamera.forward, out hit, distanceToInteract, layerMask))
            {
                StartLooking();
                holdInteractTime = InputManager.Instance.GetInteractHoldButton() ? holdInteractTime + Time.deltaTime : 0f;

                if ((InputManager.Instance.GetInteractButton() || holdInteractTime > HOLD_INTERACT_TIME) && CanPickUp())
                    PickUpObject();
            }
            else 
                StopLooking();            
        }
        else 
            StopLooking();
    }

    void StartLooking()
    {
        if (!isLooking)
        {
            isLooking = true;
            onInteractRange.Invoke(GetPickableName(), CanPickUp(), true);
        }
    }

    void StopLooking()
    {
        if (isLooking)
        {
            isLooking = false;
            onInteractRange.Invoke(GetPickableName(), CanPickUp(), false);
        }
    }

    protected abstract void PickUpObject();
    protected abstract string GetPickableName();
    protected abstract bool CanPickUp();
    public abstract PickUpType GetPickUpType();

    public InteractRangeEvent OnInteractRange
    {
        get { return onInteractRange; }
    }

    public UnityEvent OnPressed
    {
        get { return onPressed; }
    }
}