using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Enemy enemy; 
    [SerializeField] int playerLayer;
    [SerializeField] int obstacleLayer;

    public float fovAngle;
    public float raycastDistance;

    [HideInInspector] public bool seesPlayer;

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
                    if (hit.collider.gameObject.layer == playerLayer)
                    {
                        if (!enemy.isFollowingPlayer)
                        {
                            SawPlayer();
                        }
                        else if (!seesPlayer)
                        {
                            seesPlayer = true;
                            enemy.ToggleFollowingPlayer(true);
                        }
                    }
                    else if (hit.collider.gameObject.layer == obstacleLayer)
                    {
                        if (seesPlayer)
                        {
                            StopSeeingPlayer();
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == playerLayer && seesPlayer)
        {
            StopSeeingPlayer();
        }
    }

    void StopSeeingPlayer()
    {
        seesPlayer = false;
        enemy.ToggleFollowingPlayer(false);
    }

    public void SawPlayer()
    {
        enemy.isFollowingPlayer = true;
        enemy.sawPlayerBeforeHiding = false;
        enemy.ChooseNextTarget(playerTransform);
    }
}
