using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CitizenAI : MonoBehaviour
{
    Transform path;
    NavMeshAgent agent;
    GameObject currentPath;

    const float maxSpeedDelta = 1.0f;
    float maxSpeed;

    UnityEvent onRescued = new UnityEvent();

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
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        onRescued.Invoke();
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
