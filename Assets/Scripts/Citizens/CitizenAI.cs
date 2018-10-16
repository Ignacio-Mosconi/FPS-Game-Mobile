using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CitizenAI : MonoBehaviour
{
    [SerializeField] UnityEvent onRescued;

    Transform path;
    NavMeshAgent agent;
    GameObject currentPath;

    const float maxSpeedDelta = 0.5f;
    float maxSpeed;

    void Awake()
    {
        path = GameObject.Find("CitizensObjective").transform;

        agent = GetComponent<NavMeshAgent>();

        maxSpeed = agent.speed + Random.Range(-maxSpeedDelta, maxSpeedDelta);
        agent.speed = maxSpeed;

        agent.destination = path.position;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            // Here the Citizen will start to dissapear, and when the transparent is = 0 the citizen will be destroyed.

            Destroy(gameObject);
        }
    }

    public float getMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetActualSpeed()
    {
        return agent.speed;
    }

    public UnityEvent OnRescued
    {
        get { return onRescued; }
    }
}
