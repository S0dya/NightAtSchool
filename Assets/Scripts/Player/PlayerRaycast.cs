using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] Camera camera;

    [SerializeField] CanvasGroup interactionCG;
    [SerializeField] TextMeshProUGUI interactionNameText;
    [SerializeField] TextMeshProUGUI interactionButtonText;
    [SerializeField] int indexOfLayer;


    [Header("Settings")]
    public float interactionAngle;
    public float raycastDistance;

    //local
    bool isInteracting;
    Interactable curInteractable;

    Coroutine playerRaycastCor;
    
    [SerializeField] 
    List<Transform> pickableTransforms;
    Transform nearestObj;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == indexOfLayer && !pickableTransforms.Contains(collision.transform))
        {
            pickableTransforms.Add(collision.transform);
            if (pickableTransforms.Count == 1)
            {
                TogglePlayerRaycastCor(true);
            }
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == indexOfLayer && pickableTransforms.Contains(collision.transform))
        {
            pickableTransforms.Remove(collision.transform);
            if (pickableTransforms.Count == 0)
            {
                TogglePlayerRaycastCor(false);
            }
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
            float shortestDistance = Mathf.Infinity;
            Vector3 cameraPosition = camera.transform.position;

            foreach (Transform transform in pickableTransforms)
            {
                float distanceToObj = (transform.position - cameraPosition).magnitude;

                if (distanceToObj < shortestDistance)
                {
                    shortestDistance = distanceToObj;
                    nearestObj = transform;
                }
            }

            Vector3 directionToPickable = nearestObj.transform.position - cameraPosition;
            float angle = Vector3.Angle(camera.transform.forward, directionToPickable);

            if (angle < interactionAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraPosition, directionToPickable, out hit, raycastDistance))
                {
                    if (!isInteracting)
                    {
                        Interactable interactable = nearestObj.GetComponent<Interactable>();
                        curInteractable = interactable;
                        SetInteract(interactable.name, Settings.interactionNames[interactable.type]);
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
        isInteracting = true;
        interactionNameText.text = nameText;
        interactionButtonText.text = interactionText;
        GameManager.I.Open(interactionCG, 0.1f);
    }

    void UnSetInteract()
    {
        curInteractable = null;
        isInteracting = false;
        interactionNameText.text = "";
        interactionButtonText.text = "";
        GameManager.I.Close(interactionCG, 0.1f);
    }

    public void Interact()
    {
        if (curInteractable == null) return; //deler

        curInteractable.Interact();
    }
}
