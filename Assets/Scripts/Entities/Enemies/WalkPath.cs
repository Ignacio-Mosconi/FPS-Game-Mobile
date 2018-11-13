using UnityEngine;
using UnityEngine.AI;

public class WalkPath : MonoBehaviour
{
    const float LOCATION_DIFF_RADIUS = 1.5f;

    void Start()
    {
        foreach (Transform waypoint in transform)
            waypoint.position = RandomPoint(waypoint);
    }

    public int GetNumberOfWaypoints()
    {
        return transform.childCount;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return transform.GetChild(index).position;
    }

    Vector3 RandomPoint(Transform point)
    {
        Vector3 randomPoint = Random.insideUnitSphere * LOCATION_DIFF_RADIUS + point.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomPoint, out navMeshHit, LOCATION_DIFF_RADIUS * 10, -1);
        
        return new Vector3(navMeshHit.position.x, point.position.y, navMeshHit.position.z);
    }
}
