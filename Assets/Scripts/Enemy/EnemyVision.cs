using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Enemy enemy;

    public float fovAngle;
    public float raycastDistance;

    bool seesPlayer;

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
                    else
                    {
                        Debug.Log("doeasnt see ");
                        if (seesPlayer)
                        {
                            seesPlayer = false;
                            enemy.ToggleFollowingPlayer(false);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player") && seesPlayer)
        {
            seesPlayer = false;
            enemy.ToggleFollowingPlayer(false);
        }
    }

    public void SawPlayer()
    {
        enemy.isFollowingPlayer = true;
        enemy.ChooseNextTarget(playerTransform);
    }
}
