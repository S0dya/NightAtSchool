using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] float hearDistance;
    [SerializeField] int type;
    Enemy enemy;
    Interactable interactable;

    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.CompareTag("Interactable"))
        {
            switch (type)
            {
                case 0:
                    AudioManager.I.PlayOneShot(AudioManager.I.KeyDrop, transform.position);
                    break;
                case 1:
                    AudioManager.I.PlayOneShot(AudioManager.I.BatteryDrop, transform.position);
                    break;
                default: break;
            }

            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) < hearDistance
            && !enemy.isFollowingPlayer && !enemy.sawPlayerBeforeHiding)
            {
                enemy.ChooseNextTarget(transform);
            }
        }
    }
}
