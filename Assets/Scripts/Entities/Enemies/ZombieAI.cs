using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Life))]
public class ZombieAI : MonoBehaviour
{
    enum ZombieState
    {
        Wandering, Chasing, Attacking, Ivestigating, BeingHit
    }

    [Header("Zombie Attributes")]
    [SerializeField] Transform[] possibleTargets;
    [SerializeField] GameObject walkPath;
    [SerializeField] [Range(0, 100)] float viewDistance;
    [SerializeField] [Range(0, 180)] float fieldOfView;
    [SerializeField] [Range(1.75f, 5f)] float attackRange;
    [SerializeField] [Range(1, 5)] float nearDetectionDistance;
    [SerializeField] [Range(2, 25)] float investigationDistance;
    
    [Header("Animations")]
    [SerializeField] AnimationClip attackAnimation;
    
    
    const float MAX_SPEED_DELTA = 0.5f;
    const float BEING_HIT_WAIT_TIME = 1f;
    const float ATTACK_RANGE_OUT_WAIT_TIME = 0.75f;
    const float ROTATION_DEGREES_DELTA = 10f;
    const float DETECTION_TARGET_VELOCITY = 0.5f;
    
    NavMeshAgent agent;
    ZombieState currentState;
    Life zombieLife;
    GameObject attackBox;
    GameObject currentPath;
    WalkPath path;
    Transform currentTarget;
    int currentWaypointIndex;
    float maxSpeed;
    float walkSpeed;
    bool isFocusedOnTarget;

    UnityEvent onChaseStart = new UnityEvent();
    UnityEvent onChaseFinish = new UnityEvent();
    UnityEvent onAttack = new UnityEvent();

	void Awake()
    {   
        agent = GetComponent<NavMeshAgent>();
        zombieLife = GetComponent<Life>();
        attackBox = GetComponentInChildren<AttackBox>().gameObject;
        
        maxSpeed = agent.speed + Random.Range(-MAX_SPEED_DELTA, MAX_SPEED_DELTA);
        walkSpeed = maxSpeed / 3f;
        currentState = ZombieState.Wandering;
	}

    void Start()
    {
        zombieLife.OnDeath.AddListener(DisableSelf);
        zombieLife.OnDamagerHit.AddListener(StopMoving);

        foreach (Transform target in possibleTargets)
        {
            WeaponManager weaponManager = target.gameObject.GetComponentInChildren<WeaponManager>();
            
            if (weaponManager)
            {
                weaponManager.OnWeaponSwapHeadsUp.AddListener(StartListeningToNewWeapon);
                weaponManager.CurrentWeapon.OnShotHeadsUp.AddListener(StartInvestigation);
            }
        }
        
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

                RotateTowardsTarget();

                if (!IsOnAttackRange())
                {
                    LeaveTarget();
                    if (!IsInvoking("MoveAgain"))
                        Invoke("MoveAgain", ATTACK_RANGE_OUT_WAIT_TIME);
                }

                break;

            case ZombieState.Ivestigating:
                
                if (path)
                    GetRidOfPath();

                Investigate();
                SearchTarget();

                if (path)
                    currentState = ZombieState.Wandering;
                else
                    if (currentTarget)
                        currentState = ZombieState.Chasing;

                break;

            case ZombieState.BeingHit:

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
        agent.speed = walkSpeed;
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
                    possibleTarget.GetComponentInChildren<Animator>().GetFloat("Horizontal Velocity") > DETECTION_TARGET_VELOCITY)
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

    void Investigate()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            agent.speed = walkSpeed;
            CreatePath();
            onChaseFinish.Invoke();
        }
    }

    void StopMoving(float amount, Transform damager)
    {
        currentState = ZombieState.BeingHit;
        if (isFocusedOnTarget)
            LeaveTarget();
        if (!agent.isStopped)
            agent.isStopped = true;
        if (!IsInvoking("MoveAgain"))
            Invoke("MoveAgain", BEING_HIT_WAIT_TIME);
        if (!currentTarget)
            agent.destination = Vector3.MoveTowards(transform.position, damager.position, investigationDistance);
    }

    void MoveAgain()
    {
        if (zombieLife.Health > 0f)
        {
            agent.isStopped = false;
            
            if (!currentTarget)
            {
                agent.speed = maxSpeed;
                onChaseStart.Invoke();
                currentState = ZombieState.Ivestigating;
            }
            else
                currentState = IsOnAttackRange() ? ZombieState.Attacking : ZombieState.Chasing;
        }
    }

    void DisableSelf()
    {
        if (IsInvoking("MoveAgain"))
            CancelInvoke("MoveAgain");
        if (IsInvoking("Attack"))
            CancelInvoke("Attack");
        if (currentPath)
            Destroy(currentPath);
        attackBox.SetActive(false);
        agent.enabled = false;
        enabled = false;
    }

    // Attacking Methods
    void FocusOnTarget()
    {
        isFocusedOnTarget = true;
        InvokeRepeating("Attack", 0, attackAnimation.length / 2f);
    }

	void Attack()
    {
        onAttack.Invoke();

        Invoke("EnableAttackBox", attackAnimation.length / 4f);
	}

    void RotateTowardsTarget()
    {
        if (!PauseMenu.IsPaused && !LevelManager.Instance.GameOver)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                Quaternion.LookRotation(currentTarget.position - transform.position), 
                                ROTATION_DEGREES_DELTA);
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
        Invoke("DisableAttackBox", attackAnimation.length / 4f);
    }

    void DisableAttackBox()
    {
        attackBox.SetActive(false);
    }

    void StartListeningToNewWeapon(Weapon newWeapon)
    {
        newWeapon.OnShotHeadsUp.AddListener(StartInvestigation);
    }

    void StartInvestigation(Vector3 destination)
    {
        if (!currentTarget && (destination - transform.position).sqrMagnitude <= investigationDistance * investigationDistance)
        {
            agent.speed = maxSpeed;
            onChaseStart.Invoke();
            currentState = ZombieState.Ivestigating;
            if (agent.enabled && !agent.isStopped)
                agent.destination = Vector3.MoveTowards(transform.position, destination, investigationDistance);
        }
    }

    // Getters & Setters
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
