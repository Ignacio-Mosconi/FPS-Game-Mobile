using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActivateNavMesh : MonoBehaviour
{
    void Awake()
    {
        Invoke("Activate", 1f);
    }

    void Activate()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }
}
