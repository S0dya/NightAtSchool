using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Settings")]
    public float interactionAngle;
    public float raycastDistance;
    
    [Header("SerializeFields")]
    [SerializeField] Camera cam;

    [SerializeField] CanvasGroup interactionCG;
    [SerializeField] TextMeshProUGUI interactionNameText;
    [SerializeField] TextMeshProUGUI interactionButtonText;
    [SerializeField] Image interactionImage;
    [SerializeField] int interactableLayer;

    //local
    bool isInteracting;
    Interactable curInteractable;

    Coroutine playerRaycastCor;
    
    [SerializeField] 
    List<Transform> interactableTransforms;
    Transform nearestObj;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == interactableLayer && !interactableTransforms.Contains(collision.transform))
        {
            interactableTransforms.Add(collision.transform);
            if (interactableTransforms.Count == 1)
            {
                TogglePlayerRaycastCor(true);
            }
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == interactableLayer && interactableTransforms.Contains(collision.transform))
        {
            RemoveTransform(collision.transform);
        }
    }

    void TogglePlayerRaycastCor(bool val)
    {
        if (val) playerRaycastCor = StartCoroutine(PlayerRaycastCor());
        else if (playerRaycastCor != null) StopCoroutine(playerRaycastCor);
    }

    IEnumerator PlayerRaycastCor()
    {
        while (true)
        {
            float shortestAngle = Mathf.Infinity;
            Vector3 cameraPosition = cam.transform.position;

            foreach (Transform transform in interactableTransforms)
            {
                Vector3 directionToCurPickable = transform.position - cameraPosition;
                float curAngle = Vector3.Angle(cam.transform.forward, directionToCurPickable);

                if (curAngle < shortestAngle)
                {
                    shortestAngle = curAngle;
                    nearestObj = transform;
                }
            }

            Vector3 directionToPickable = nearestObj.position - cameraPosition;
            float angle = Vector3.Angle(cam.transform.forward, directionToPickable);
            float angleMultiplayer = nearestObj.gameObject.CompareTag("HideInteractable") ? 3.5f : 1;

            if (angle < interactionAngle * angleMultiplayer)
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraPosition, directionToPickable, out hit, raycastDistance))
                {
                    if (!isInteracting)
                    {
                        Interactable interactable = nearestObj.GetComponent<Interactable>();
                        curInteractable = interactable;
                        SetInteract(interactable.nameOfObj, Settings.interactionNames[interactable.type]);
                    }
                }
                else if (isInteracting)
                {
                    UnSetInteract();
                }
            }
            else if (isInteracting)
            {
                UnSetInteract();
            }

            yield return null;
        }
    }

    void SetInteract(string nameText, string interactionText)
    {
        ToggleInteraction(nameText, interactionText);
        GameManager.I.Open(interactionCG, 0.3f);
    }

    void UnSetInteract()
    {
        ToggleInteraction(null, null);
        curInteractable = null;
        GameManager.I.Close(interactionCG, 0.3f);
    }

    public void Interact()
    {
        if (curInteractable == null) return; //deler

        curInteractable.Interact();

        RemoveTransform(nearestObj);
    }

    void RemoveTransform(Transform transform)
    {
        interactableTransforms.Remove(transform);
        if (interactableTransforms.Count == 0)
        {
            TogglePlayerRaycastCor(false);

            if (isInteracting) UnSetInteract();
        }
    }
    
    void ToggleInteraction(string nameText, string interactionText)
    {
        isInteracting = nameText != null;
        interactionImage.enabled = nameText != null;
        interactionNameText.text = nameText;
        interactionButtonText.text = interactionText;
    }
}
