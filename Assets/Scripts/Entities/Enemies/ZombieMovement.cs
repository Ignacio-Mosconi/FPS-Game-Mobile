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
    [SerializeField] float differenceRadius;
    [SerializeField] float detectionRadius;
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
    Transform followingTarget;
    int currentWaypointIndex;
    float maxSpeed;

	void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAttacking = GetComponent<ZombieAttacking>();
        maxSpeed = agent.speed;

        InvokeRepeating("SearchTarget", 0, timeBetweenSearches);
	}
	
	void Update()
    {
        if (followingTarget)
        {
            if (path)
            {
                zombieBreathSound.Stop();
                zombieChaseSound.Play();

                Destroy(currentPath);
                path = null;
                agent.speed = maxSpeed;
            }
            if ((transform.position - followingTarget.position).sqrMagnitude <= attackRange * attackRange)
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

                currentPath = Instantiate(walkPath, transform.position, Quaternion.LookRotation(transform.forward));
                path = currentPath.GetComponent<WalkPath>();
                agent.speed = maxSpeed / 3;
                agent.destination = transform.position;
                currentWaypointIndex = 0;
            }
            Wander();
        }
	}

    void SearchTarget()
    {
        float closestDistance = detectionRadius;

        for (int i = 0; i < possibleTargets.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, possibleTargets[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                followingTarget = possibleTargets[i];
            }
        }

        if (closestDistance == detectionRadius)
            followingTarget = null;
    }

    void ChaseTarget()
    {
        Vector3 targetDiff = followingTarget.position - agent.destination;

        if (targetDiff.sqrMagnitude > differenceRadius * differenceRadius)
            agent.destination = followingTarget.position;
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
        get { return followingTarget; }
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
