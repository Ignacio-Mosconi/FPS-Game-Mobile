using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform fpsCamera;
    [SerializeField] float rotationSpeed;
    [SerializeField] float verticalViewRange;
    float verAngle = 0;

    void Update()
    {
        float horRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float verRotation = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        verAngle += verRotation;
        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
        transform.Rotate(0, horRotation, 0);
    }
}
