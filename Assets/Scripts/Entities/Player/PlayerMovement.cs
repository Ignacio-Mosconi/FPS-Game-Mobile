using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum WalkingSurface
{
	Indoors, Outdoors
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(MeshFilter))]

public class PlayerMovement : MonoBehaviour 
{
    [Header("Movement Attributes")]
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpingSpeed;
    [SerializeField] float gravity;
    [SerializeField] float pushingForce;
    [SerializeField] float maxFallingDistance;
    [Header("Events")]
    [SerializeField] UnityEvent onSurfaceChange;
    Life life;
    CharacterController charController;
    const float FALLING_DAMAGE_MULTIPLIER = 10;
    float verticalSpeed;
    float distanceToGround;
    bool jumpedWhileSprinting;
    bool pressedSprintModifier;
    float fallingDistance;
    float lastPositionY;
    WalkingSurface currentSurface;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        life = GetComponent<Life>();
        distanceToGround = GetComponent<MeshFilter>().mesh.bounds.extents.y + 0.5f;      
        jumpedWhileSprinting = false;
        currentSurface = WalkingSurface.Outdoors;
    }

    void Update() 
	{
        Vector3 movement = new Vector3(0, 0, 0);

        verticalSpeed -= gravity * Time.deltaTime;

        float fwdMovement = InputManager.Instance.GetVerticalAxis();
        float horMovement = InputManager.Instance.GetHorizontalAxis();
        float speedMultiplier = (fwdMovement > 0 && InputManager.Instance.GetSprintButton() &&
                                !IsJumping()) ||  jumpedWhileSprinting  ||
                                 (pressedSprintModifier && fwdMovement > 0 ) ? 1.0f : 0.5f;

        Vector3 inputVector = new Vector3(horMovement, 0, fwdMovement);
        inputVector.Normalize();
        inputVector *= movementSpeed;

        movement += (transform.forward * inputVector.z + transform.right * inputVector.x) * speedMultiplier;
        movement += Vector3.up * verticalSpeed;

        charController.Move(movement * Time.deltaTime);

        if (lastPositionY > transform.position.y)
            fallingDistance += lastPositionY - transform.position.y;
        lastPositionY = transform.position.y;

        if (charController.isGrounded)
        {
            if (fallingDistance >= maxFallingDistance)
                life.TakeDamage(FALLING_DAMAGE_MULTIPLIER * fallingDistance);

            fallingDistance = 0;
            lastPositionY = 0;

            verticalSpeed = InputManager.Instance.GetJumpButton() ? jumpingSpeed : 0;
            jumpedWhileSprinting = InputManager.Instance.GetJumpButton() && fwdMovement > 0 && 
                                    speedMultiplier > 0.5f ? true : false;
            pressedSprintModifier = InputManager.Instance.GetSprintButtonModifier() ||
                                    (pressedSprintModifier && speedMultiplier == 1.0f) ? true : false;
        }
        else
            if ((charController.collisionFlags & CollisionFlags.Above) != 0)
                verticalSpeed = 0;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.rigidbody;

        if (hitRigidbody && !hitRigidbody.isKinematic)
        {
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            hitRigidbody.velocity = pushDirection * pushingForce;
        }
        
        WalkingSurface previousSurface = currentSurface;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            currentSurface = WalkingSurface.Outdoors;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Buildings") || 
            hit.collider.gameObject.layer == LayerMask.NameToLayer("Decoration"))  
            currentSurface = WalkingSurface.Indoors;

        if (currentSurface != previousSurface)
            onSurfaceChange.Invoke();
    }

    public bool IsJumping()
    {
        return !(Physics.Raycast(transform.position, -Vector3.up, distanceToGround));
    }

    public int GetCurrentSurfaceIndex()
    {
        return (int)currentSurface;
    }

    public float MovementSpeed
    {
        get { return movementSpeed;}
    }

    public UnityEvent OnSurfaceChange
    {
        get { return onSurfaceChange; }
    }
}
