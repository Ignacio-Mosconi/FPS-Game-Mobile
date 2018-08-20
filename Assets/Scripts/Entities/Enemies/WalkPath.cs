using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{
    public int GetNumberOfWaypoints()
    {
        return transform.childCount;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return transform.GetChild(index).position;
    }
}
