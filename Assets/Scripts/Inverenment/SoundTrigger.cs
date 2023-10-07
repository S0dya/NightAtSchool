using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] float hearDistance;
    Enemy enemy;

    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.CompareTag("Interactable"))
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < hearDistance
            && !enemy.isFollowingPlayer && !enemy.sawPlayerBeforeHiding)
            {
                enemy.ChooseNextTarget(transform);
            }
        }
    }
}
