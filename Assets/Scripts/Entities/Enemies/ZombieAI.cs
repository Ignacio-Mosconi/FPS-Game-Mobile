using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Life))]
public class ZombieAI : MonoBehaviour
{
    enum ZombieState
    {
        Wandering, Chasing, Attacking, BeingHit, Dead
    }

    [Header("Zombie Attributes")]
    [SerializeField] Transform[] possibleTargets;
    [SerializeField] [Range(0, 100)] float viewDistance;
    [SerializeField] [Range(0, 180)] float fieldOfView;
    [SerializeField] [Range(1.75f, 5f)] float attackRange;
    [SerializeField] [Range(1, 5)] float nearDetectionDistance;
    [SerializeField] GameObject walkPath;
    [Header("Animations")]
    [SerializeField] AnimationClip attackAnimation;
    [Header("Events")]
    [SerializeField] UnityEvent onChaseStart;
    [SerializeField] UnityEvent onChaseFinish;
    [SerializeField] UnityEvent onAttack;
    const float maxSpeedDelta = 0.5f;
    NavMeshAgent agent;
    ZombieState currentState;
    Life zombieLife;
    GameObject attackBox;
    GameObject currentPath;
    WalkPath path;
    Transform currentTarget;
    int currentWaypointIndex;
    float maxSpeed;
    bool isFocusedOnTarget;

	void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieLife = GetComponent<Life>();
        attackBox = GetComponentInChildren<AttackBox>().gameObject;
        
        maxSpeed = agent.speed + Random.Range(-maxSpeedDelta, maxSpeedDelta);
        currentState = ZombieState.Wandering;
	}

    void Start()
    {
        zombieLife.OnDeath.AddListener(StopMoving);
        zombieLife.OnHit.AddListener(StopMoving);
        
        CreatePath();
    }
	
    void Update()
    {
        switch (currentState)
        {
            case ZombieState.Wandering:

                if (!path)
                {
                    onChaseFinish.Invoke();
                    CreatePath();
                }
                Wander();
                SearchTarget();
                
                if (currentTarget)
                    currentState = ZombieState.Chasing;
                
                break;

            case ZombieState.Chasing:

                if (path)
                {
                    OnChaseStart.Invoke();
                    GetRidOfPath();
                }

                ChaseTarget();

                if (!currentTarget)
                    currentState = ZombieState.Wandering;
                else 
                    if (IsOnAttackRange())
                        {
                            agent.isStopped = true;
                            currentState = ZombieState.Attacking;
                        }

                    break;

            case ZombieState.Attacking:
                
                if (!isFocusedOnTarget)
                    FocusOnTarget();

                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                    Quaternion.LookRotation(currentTarget.position - transform.position), 10);

                if (!IsOnAttackRange())
                {
                    LeaveTarget();
                    if (!IsInvoking("MoveAgain"))
                        Invoke("MoveAgain", 1);
                }

                break;

            case ZombieState.BeingHit:

                if (!agent.isStopped)
                    agent.isStopped = true;
                if (!IsInvoking("MoveAgain"))
                    Invoke("MoveAgain", 1);

                break;

            case ZombieState.Dead:

                if (IsInvoking("MoveAgain"))
                    CancelInvoke("MoveAgain");
                enabled = false;
                agent.enabled = false;
                if (currentPath)
                    Destroy(currentPath);

                break;
        }
    }

    // Movement Methods
    bool IsOnAttackRange()
    {
        return (transform.position - currentTarget.position).sqrMagnitude <= attackRange * attackRange;
    }

    void CreatePath()
    {
        currentPath = Instantiate(walkPath, transform.position, Quaternion.LookRotation(transform.forward), transform.parent);
        path = currentPath.GetComponent<WalkPath>();
        agent.speed = maxSpeed / 3;
        agent.destination = transform.position;
        currentWaypointIndex = 0;
    }

    void GetRidOfPath()
    {
        Destroy(currentPath);
        path = null;
        agent.speed = maxSpeed;
    }

    void SearchTarget()
    {
        float closestTargetDistance = viewDistance;

        foreach (Transform possibleTarget in possibleTargets)
        {
            float distanceToTarget = Vector3.Distance(possibleTarget.position, transform.position);

            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(possibleTarget.position)) < fieldOfView / 2f)
            {              
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
            else
            {
                if (distanceToTarget < nearDetectionDistance && 
                    possibleTarget.GetComponentInChildren<Animator>().GetFloat("Horizontal Velocity") > 0.5f)
                {
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

    // Attacking Methods
    void FocusOnTarget()
    {
        isFocusedOnTarget = true;
        InvokeRepeating("Attack", 0, attackAnimation.length / 2);
    }

	void Attack()
    {
        onAttack.Invoke();

        Invoke("EnableAttackBox", attackAnimation.length / 4);
	}

    void LeaveTarget()
    {
        isFocusedOnTarget = false;
        attackBox.SetActive(false);
        CancelInvoke("Attack");
        CancelInvoke("EnableAttackBox");
    }

    void EnableAttackBox()
    {
        attackBox.SetActive(true);
        Invoke("DisableAttackBox", attackAnimation.length / 4);
    }

    void DisableAttackBox()
    {
        attackBox.SetActive(false);
    }

    void StopMoving()
    {
        if (currentState == ZombieState.Dead)
            return;

        if (currentState == ZombieState.Attacking)
            LeaveTarget();
        currentState = zombieLife.Health > 1f ? ZombieState.BeingHit : ZombieState.Dead;
    }

    void MoveAgain()
    {
        agent.isStopped = false;
        
        if (!currentTarget)
            currentState = ZombieState.Wandering;
        else
            currentState = IsOnAttackRange() ? ZombieState.Attacking : ZombieState.Chasing;
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

    public UnityEvent OnAttack
    {
        get { return onAttack; }
    }
}
