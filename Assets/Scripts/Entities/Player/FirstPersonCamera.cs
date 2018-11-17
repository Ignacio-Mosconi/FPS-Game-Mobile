using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Viewing Attributes")]
    [SerializeField] [Range(45f, 360f)] float maxRotationSpeed = 180f;
    [SerializeField] [Range(45f, 90f)] float verticalViewRange = 90f;
    [SerializeField] AnimationCurve touchDeltaCurve;
    
    Transform fpsCamera;
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
        maxRotationSpeed *= 0.75f;
#endif
        fpsCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        float horRotation = InputManager.Instance.GetHorizontalViewAxis();
        float verRotation = InputManager.Instance.GetVerticalViewAxis();

#if UNITY_STANDALONE
        if (InputManager.Instance.ControllerConnected)
        {
            Vector2 rotation = new Vector2(horRotation, verRotation);
            if (rotation.sqrMagnitude > 1f)
                rotation.Normalize();
            rotationSpeed = maxRotationSpeed * rotation.magnitude;
        }
        else
            rotationSpeed = maxRotationSpeed;
#else
        Vector2 rotation = new Vector2(horRotation, verRotation);
        rotationSpeed = maxRotationSpeed * rotation.magnitude * touchDeltaCurve;
#endif

        horAngle += horRotation * rotationSpeed * Time.deltaTime;
        verAngle -= verRotation * rotationSpeed * Time.deltaTime;

        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        transform.eulerAngles = new Vector3(0, horAngle, 0);
        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
    }
}