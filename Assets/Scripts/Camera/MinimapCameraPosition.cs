using UnityEngine;

public class MinimapCameraPosition : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float heightOffset;

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y += heightOffset;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
