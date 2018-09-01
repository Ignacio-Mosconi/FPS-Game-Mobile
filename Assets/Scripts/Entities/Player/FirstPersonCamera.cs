using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Viewing Attributes")]
    [SerializeField] [Range(45, 360)] float rotationSpeed;
    [SerializeField] [Range(45, 90)] float verticalViewRange;
    Transform fpsCamera;
    float verAngle = 0;
    float horAngle = 0;

    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        float horRotation = InputManager.Instance.GetHorizontalViewAxis() * rotationSpeed * Time.deltaTime;
        float verRotation = InputManager.Instance.GetVerticalViewAxis() * rotationSpeed * Time.deltaTime;

        horAngle += horRotation;
        verAngle -= verRotation;

        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        transform.localEulerAngles = new Vector3(0, horAngle, 0);
        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
    }
}
