using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("SerializeFields")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Player player;
    [SerializeField] Transform playerTransform;

    [SerializeField] List<Transform> patrolPoints;

    [Header("Settings")]
    public float stopDistance;
    public float chooseRandomTargetMin;
    public float chooseRandomTargetMax;

    public float fovAngle;
    public float raycastDistance;


    //coroutines
    Coroutine walkCor;
    Coroutine chooseNextTargetCor;

    //public
    [HideInInspector] bool seesPlayer;

    //local
    Transform target;
    Vector3 targetPosition;
    int curPatrolPointIndex = 0;

    bool isIdle = true;

    void Awake()
    {

    }

    void Start()
    {
        ChooseNextTarget();
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
                break;
            }

            yield return null;
        }

        ChooseNextTarget();
    }

    void ChooseNextTarget()
    {
        if (chooseNextTargetCor != null) StopCoroutine(chooseNextTargetCor);
        chooseNextTargetCor = StartCoroutine(ChooseNextTargetCor());
    }
    IEnumerator ChooseNextTargetCor()
    {
        if (seesPlayer)
        {
            target = playerTransform;
        }
        else
        {
            if (walkCor != null) StopCoroutine(walkCor);
            animator.Play("IDLE");
            isIdle = true;

            yield return new WaitForSeconds(Random.Range(chooseRandomTargetMin, chooseRandomTargetMax));

            Transform temp = patrolPoints[^1];
            patrolPoints[^1] = patrolPoints[curPatrolPointIndex];
            patrolPoints[curPatrolPointIndex] = temp;

            curPatrolPointIndex = Random.Range(0, patrolPoints.Count - 1);
            Debug.Log(curPatrolPointIndex);
            target = patrolPoints[curPatrolPointIndex];
        }

        StartWalking();
    }

    //enemy - player
    void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float angle = Vector3.Angle(directionToPlayer, transform.forward);

            if (angle <= fovAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, raycastDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player detected!");
                        seesPlayer = true;
                        ChooseNextTarget();
                    }
                    else
                    {
                        Debug.Log("Obstacle detected.");
                    }
                }
            }
        }
    }
}
