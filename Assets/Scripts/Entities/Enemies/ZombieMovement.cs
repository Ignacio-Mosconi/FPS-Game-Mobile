using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]

public class ZombieMovement : MonoBehaviour
{
    [Header("Zombie Attributes")]
    [SerializeField] Transform[] possibleTargets;
    [SerializeField] [Range(0, 100)] float viewDistance;
    [SerializeField] [Range(0, 180)] float fieldOfView;
    [SerializeField] [Range(2, 5)] float attackRange;
    [SerializeField] GameObject walkPath;
    [Header("Events")]
    [SerializeField] UnityEvent onChaseStart;
    [SerializeField] UnityEvent onChaseFinish;
    [SerializeField] UnityEvent onAttackRange;
    [SerializeField] UnityEvent outOfAttackRange;
    [Header("Sounds")]
    NavMeshAgent agent;
    ZombieAttacking zombieAttacking;
    GameObject currentPath;
    WalkPath path;
    Transform currentTarget;
    int currentWaypointIndex;
    float maxSpeed;

	void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAttacking = GetComponent<ZombieAttacking>();
        maxSpeed = agent.speed + Random.Range(-0.5f, 0.5f);
	}
	
	void Update()
    {
        if (currentTarget)
        {
            if (path)
            {
                onChaseStart.Invoke();

                Destroy(currentPath);
                path = null;
                agent.speed = maxSpeed;
            }
            if ((transform.position - currentTarget.position).sqrMagnitude <= attackRange * attackRange)
            {
                if (!zombieAttacking.IsAttacking)
                {
                    onAttackRange.Invoke();
                    agent.isStopped = true;
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                    Quaternion.LookRotation(currentTarget.position - transform.position), 10);
            }
            else
            {
                if (zombieAttacking.IsAttacking)
                {
                    outOfAttackRange.Invoke();
                    agent.isStopped = false;
                }
                ChaseTarget();
            }
        }
        else
        {
            if (!path)
            {
                OnChaseFinish.Invoke();

                currentPath = Instantiate(walkPath, transform.position, Quaternion.LookRotation(transform.forward), transform.parent);
                path = currentPath.GetComponent<WalkPath>();
                agent.speed = maxSpeed / 3;
                agent.destination = transform.position;
                currentWaypointIndex = 0;
            }
            Wander();
            SearchTarget();
        }
	}

    void SearchTarget()
    {
        float closestTargetDistance = viewDistance;

        foreach (Transform possibleTarget in possibleTargets)
        {
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(possibleTarget.position)) < fieldOfView / 2f)
            {
                float distanceToTarget = Vector3.Distance(possibleTarget.position, transform.position);
                
                if (distanceToTarget < closestTargetDistance)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(transform.position, possibleTarget.position, out hit))
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            closestTargetDistance = distanceToTarget;
                            currentTarget = possibleTarget;
                        }
                }
            }
        }
    }  

    void ChaseTarget()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) > viewDistance)
            currentTarget = null;
        else
            agent.destination = currentTarget.position;
    }

    void Wander()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypointIndex++;
            currentWaypointIndex %= path.GetNumberOfWaypoints();
            agent.destination = path.GetWaypointPosition(currentWaypointIndex);
        }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    public Transform CurrentTarget
    {
        get { return currentTarget; }
    }

    public UnityEvent OnChaseStart
    {
        get { return onChaseStart; }
    }

    public UnityEvent OnChaseFinish
    {
        get { return onChaseFinish; }
    }

    public UnityEvent OnAttackRange
    {
        get { return onAttackRange; }
    }

    public UnityEvent OutOfAttackRange
    {
        get { return outOfAttackRange; }
    }
}
