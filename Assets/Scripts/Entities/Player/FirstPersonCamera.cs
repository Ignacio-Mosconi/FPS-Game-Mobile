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

    void Awake()
    {
        #if UNITY_STANDALONE
            GameManager.Instance.HideCursor();
        #endif
    }

    void Start()
    {
        fpsCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        float horRotation = InputManager.Instance.GetHorizontalViewAxis() * rotationSpeed;
        float verRotation = InputManager.Instance.GetVerticalViewAxis() * rotationSpeed;

        horAngle += horRotation * Time.deltaTime;
        verAngle -= verRotation * Time.deltaTime;

        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        transform.localEulerAngles = new Vector3(0, horAngle, 0);
        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
    }
}
