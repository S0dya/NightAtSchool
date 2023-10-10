using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] float hearDistance;
    [SerializeField] int type;
    public CapsuleCollider capsuleC;
    public BoxCollider boxC;
    Enemy enemy;
    Interactable interactable;

    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    void OnTriggerEnter(Collider collision)
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

            ToggleCollider(false);
        }
    }
    public void ToggleCollider(bool val)
    {
        if (capsuleC != null) capsuleC.enabled = val;
        else if (boxC != null) boxC.enabled = val;
    }
}
