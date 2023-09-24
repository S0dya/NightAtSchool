using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] CanvasGroup interactionTextCG;
    [SerializeField] TextMeshProUGUI interactionText;

    public float raycastDistance;
    bool isInteracting;

    void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == interactableLayer)
        {
            Vector3 directionToPickable = collision.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToPickable);

            Debug.Log("1");

            if (angle < 45f)
            {
                Debug.Log("2");
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPickable, out hit, raycastDistance))
                {
                    Debug.Log("3");
                    if (hit.collider.CompareTag("Pickable") || hit.collider.CompareTag("Interactable"))
                    {
                        Debug.Log("4");
                        if (!isInteracting)
                        {
                            Debug.Log("5");
                            Interactable interactable = collision.gameObject.GetComponent<Interactable>();
                            SetInteract(interactable.actionName);
                        }
                    }
                    else
                    {
                        Debug.Log("6");
                        if (isInteracting)
                        {
                            Debug.Log("7");
                            UnSetInteract();
                        }
                    }
                }
            }
        }
    }

    void SetInteract(string text)
    {
        isInteracting = true;
        interactionText.text = text;
        GameManager.I.FadeIn(interactionTextCG, 0.1f);
    }

    void UnSetInteract()
    {
        isInteracting = false;
        interactionText.text = "";
        GameManager.I.FadeOut(interactionTextCG, 0.1f);
    }
}
