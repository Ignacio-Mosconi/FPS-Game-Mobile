using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] Transform[] possibleTargets;
    [SerializeField] float timeBetweenSearches;
    [SerializeField] float chasingDiffDistance;
    [SerializeField] float viewDistance;
    [SerializeField] float fieldOfView;
    [SerializeField] float attackRange;
    [SerializeField] GameObject walkPath;
    [SerializeField] UnityEvent onAttackRange;
    [SerializeField] UnityEvent outOfAttackRange;
    [SerializeField] AudioSource zombieBreathSound;
    [SerializeField] AudioSource zombieChaseSound;
    ZombieAttacking zombieAttacking;
    GameObject currentPath;
    WalkPath path;
    NavMeshAgent agent;
    Transform currentTarget;
    int currentWaypointIndex;
    float maxSpeed;

	void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAttacking = GetComponent<ZombieAttacking>();
        maxSpeed = agent.speed;
	}
	
	void Update()
    {
        if (currentTarget)
        {
            if (path)
            {
                zombieBreathSound.Stop();
                zombieChaseSound.Play();

                Destroy(currentPath);
                path = null;
                agent.speed = maxSpeed;
            }
            if ((transform.position - currentTarget.position).sqrMagnitude <= attackRange * attackRange)
            {
                if (!zombieAttacking.IsAttacking)
                {
                    onAttackRange.Invoke();
                    zombieChaseSound.Stop();
                    agent.isStopped = true;
                }
            }
            else
            {
                if (zombieAttacking.IsAttacking)
                {
                    outOfAttackRange.Invoke();
                    zombieChaseSound.Play();
                    agent.isStopped = false;
                }
                ChaseTarget();
            }
        }
        else
        {
            if (!path)
            {
                zombieChaseSound.Stop();
                zombieBreathSound.Play();

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
        {
            Vector3 targetDiff = currentTarget.position - agent.destination;

            if (targetDiff.sqrMagnitude > chasingDiffDistance * chasingDiffDistance)
                agent.destination = currentTarget.position;
        }
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

    public Transform FollowingTarget
    {
        get { return currentTarget; }
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
