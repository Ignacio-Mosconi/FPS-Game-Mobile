using UnityEngine;
using EZCameraShake;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Viewing Attributes")]
    [SerializeField] [Range(45f, 360f)] float maxRotationSpeed = 180f;
    [SerializeField] [Range(45f, 90f)] float verticalViewRange = 90f;
#if UNITY_ANDROID
    [SerializeField] AnimationCurve touchDeltaCurve;
#endif

    const float MIN_CAM_SHAKE = 2.5f;
    const float MAX_CAM_SHAKE = 7f;
    const float MAX_CAM_SHAKE_DMG_AMOUNT = 20f;

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
        gameObject.GetComponentInParent<Life>().OnDamagerHit.AddListener(ShakeCamera);
    }

    void Update()
    {
        float horRotation = InputManager.Instance.GetHorizontalViewAxis();
        float verRotation = InputManager.Instance.GetVerticalViewAxis();

#if UNITY_STANDALONE
        if (InputManager.Instance.ControllerConnected)
        {
            Vector2 rotation = new Vector2(horRotation, verRotation);
            rotationSpeed = maxRotationSpeed * rotation.magnitude;
        }
        else
            rotationSpeed = maxRotationSpeed;
#else
        Vector2 rotation = new Vector2(horRotation, verRotation);
        rotationSpeed = maxRotationSpeed * touchDeltaCurve.Evaluate(rotation.magnitude);
#endif

        horAngle += horRotation * rotationSpeed * Time.deltaTime;
        verAngle -= verRotation * rotationSpeed * Time.deltaTime;

        verAngle = Mathf.Clamp(verAngle, -verticalViewRange, verticalViewRange);

        transform.eulerAngles = new Vector3(0, horAngle, 0);
        fpsCamera.localEulerAngles = new Vector3(verAngle, 0, 0);
    }

    void ShakeCamera(float amount, Transform damager)
    {
        float shakeMagnitudeMult = Mathf.Clamp01(amount / MAX_CAM_SHAKE_DMG_AMOUNT);
        float shakeMagnitude = Random.Range(MIN_CAM_SHAKE, MAX_CAM_SHAKE) * shakeMagnitudeMult;

        CameraShaker.Instance.ShakeOnce(shakeMagnitude, 2.5f, 0.15f, 1f);
    }
}