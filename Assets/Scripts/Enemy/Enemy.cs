using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : SingletonMonobehaviour<Enemy> //only one enemy
{
    [Header("Settings")]
    public float stopDistance;
    public float chooseRandomTargetMinTime;
    public float chooseRandomTargetMaxTime;

    public float stopFollowingPlayerMaxTime;
    public float stopFollowingPlayerMinTime;

    [Header("SerializeFields")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Player player;
    [SerializeField] Transform playerTransform;
    public EnemyVision enemyVision;

    [SerializeField] List<Transform> patrolPoints;

    //coroutines
    Coroutine walkCor;
    Coroutine chooseNextTargetCor;
    Coroutine stopFollowingPlayerCor;

    //public
    [HideInInspector] public bool isFollowingPlayer;
    [HideInInspector] public bool heardSound;
    [HideInInspector] public bool sawPlayerBeforeHiding;

    //local
    Transform target;
    Vector3 targetPosition;
    int curPatrolPointIndex = 0;

    Vector3 velocity;
    Vector3 lastPosition;

    bool isIdle = true;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        ChooseNextTarget(null);
    }

    void Update()
    {
        Vector3 currentPosition = agent.transform.position;
        velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        if (!isIdle)
        {
            animator.speed = velocity.magnitude / 4;
        }
    }

    //walking
    public void StartWalking()
    {
        if (walkCor != null) StopCoroutine(walkCor);
        walkCor = StartCoroutine(WalkCor());
    }
    IEnumerator WalkCor()
    {
        if (isIdle)
        {
            animator.Play("Walking");
            isIdle = false;
        }

        while (true)
        {
            targetPosition = target.position;
            agent.SetDestination(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                if (sawPlayerBeforeHiding)
                {
                    ScreamerUI.I.PlayScreamer();
                    Destroy(gameObject);
                }
                break;
            }

            yield return null;
        }

        ChooseNextTarget(null);
    }

    public void ChooseNextTarget(Transform transform)
    {
        if (chooseNextTargetCor != null) StopCoroutine(chooseNextTargetCor);

        if (transform != null)
        {
            target = transform;

            StartWalking();
        }
        else
        {
            chooseNextTargetCor = StartCoroutine(ChooseNextTargetCor());
        }
    }
    IEnumerator ChooseNextTargetCor()
    {
        if (walkCor != null) StopCoroutine(walkCor);
        animator.Play("IDLE");
        isIdle = true;
        animator.speed = 1;

        yield return new WaitForSeconds(Random.Range(chooseRandomTargetMinTime, chooseRandomTargetMaxTime));

        Transform temp = patrolPoints[^1];
        patrolPoints[^1] = patrolPoints[curPatrolPointIndex];
        patrolPoints[curPatrolPointIndex] = temp;

        curPatrolPointIndex = Random.Range(0, patrolPoints.Count - 1);
        target = patrolPoints[curPatrolPointIndex];

        StartWalking();
    }

    //soundsTrigger
    public void OnHearSound(Transform transform)
    {
        if (!isFollowingPlayer)
        {
            ChooseNextTarget(transform);
        }
    }

    //enemy - player
    public void ToggleFollowingPlayer(bool val)
    {
        if (val)
        {
            if (stopFollowingPlayerCor != null) StopCoroutine(stopFollowingPlayerCor);
            ChooseNextTarget(playerTransform);
        }
        else
        {
            stopFollowingPlayerCor = StartCoroutine(StopFollowingPlayerCor());
        }
    }
    IEnumerator StopFollowingPlayerCor()
    {
        yield return new WaitForSeconds(Random.Range(stopFollowingPlayerMinTime, stopFollowingPlayerMaxTime));
        isFollowingPlayer = false;
        target = transform;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && isFollowingPlayer && enemyVision.seesPlayer)
        {
            ScreamerUI.I.PlayScreamer();
            Destroy(gameObject);
        }
    }
}
