using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshFilter))]

public class PlayerMovement : MonoBehaviour 
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpingSpeed;
    [SerializeField] float gravity;
    [SerializeField] float pushingForce;
    [SerializeField] float maxFallingDistance;
    [SerializeField] AudioSource indoorsFootstepsSound;
    [SerializeField] AudioSource outdoorsFootstepsSound;
    Life life;
    CharacterController charController;
    float verticalSpeed;
    float distanceToGround;
    bool jumpedWhileSprinting;
    float fallingDistance;
    float lastPositionY;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        life = GetComponent<Life>();
        distanceToGround = GetComponent<MeshFilter>().mesh.bounds.extents.y + 0.5f;
        jumpedWhileSprinting = false;
    }

    void Update() 
	{
        Vector3 movement = new Vector3(0, 0, 0);

        verticalSpeed -= gravity * Time.deltaTime;

        float fwdMovement = Input.GetAxis("Vertical");
        float horMovement = Input.GetAxis("Horizontal");
        float speedMultiplier = (Input.GetAxis("Vertical") > 0 && Input.GetButton("Sprint") && !IsJumping()) || jumpedWhileSprinting ?
            1.0f : 0.5f;

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
                life.TakeDamage(10 * fallingDistance);

            fallingDistance = 0;
            lastPositionY = 0;

            verticalSpeed = (Input.GetButton("Jump")) ? jumpingSpeed : 0;
            jumpedWhileSprinting = Input.GetButton("Jump") && Input.GetAxis("Vertical") > 0 && Input.GetButton("Sprint") ? true : false;
        }
        else
        {
            if ((charController.collisionFlags & CollisionFlags.Above) != 0)
                verticalSpeed = 0;
            if (outdoorsFootstepsSound.isPlaying)
                outdoorsFootstepsSound.Stop();
            if (indoorsFootstepsSound.isPlaying)
                indoorsFootstepsSound.Stop();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.rigidbody;

        if (hitRigidbody && !hitRigidbody.isKinematic)
        {
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            hitRigidbody.velocity = pushDirection * pushingForce;
        }

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            PlayFootstepsSound(outdoorsFootstepsSound);
        else
            outdoorsFootstepsSound.Stop();

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Buildings") ||
            hit.collider.gameObject.layer == LayerMask.NameToLayer("Decoration"))
            PlayFootstepsSound(indoorsFootstepsSound);
        else
            indoorsFootstepsSound.Stop();
    }

    void PlayFootstepsSound(AudioSource footsteps)
    {
        Vector3 horizontalVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);

        if (charController.isGrounded && horizontalVelocity.sqrMagnitude > 0)
        {
            footsteps.pitch = (horizontalVelocity.sqrMagnitude > movementSpeed / 2 * movementSpeed / 2) ? 2.0f : 1.0f;

            if (!footsteps.isPlaying)
                footsteps.Play();
        }
        else
            footsteps.Stop();
    }

    public bool IsJumping()
    {
        return !(Physics.Raycast(transform.position, -Vector3.up, distanceToGround));
    }

    public float MovementSpeed
    {
        get { return movementSpeed;}
    }
}
