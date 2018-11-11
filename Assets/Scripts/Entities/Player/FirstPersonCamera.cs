using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Viewing Attributes")]
    [SerializeField] [Range(45f, 360f)] float maxRotationSpeed = 180f;
    [SerializeField] [Range(45f, 90f)] float verticalViewRange = 90f;
    [SerializeField] [Range(5f, 50f)] float touchSensitivity = 30f;
    
    Transform fpsCamera;
    TouchPad aimTouchPad;
    float rotationSpeed;
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
        #if UNITY_ANDROID
            aimTouchPad = FindObjectOfType<TouchPad>();
        #endif
        fpsCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        #if UNITY_ANDROID            
            rotationSpeed = aimTouchPad.PointerDelta.magnitude * touchSensitivity;
            rotationSpeed = rotationSpeed > maxRotationSpeed ? maxRotationSpeed : rotationSpeed;
        #else
            rotationSpeed = maxRotationSpeed;
        #endif

        float horRotation = InputManager.Instance.GetHorizontalViewAxis() * rotationSpeed;
        float verRotation = InputManager.Instance.GetVerticalViewAxis() * rotationSpeed;

        horAngle += horRotation * Time.deltaTime;
        verAngle -= verRotation * Time.deltaTime;

        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        transform.eulerAngles = new Vector3(0, horAngle, 0);
        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
    }
}
