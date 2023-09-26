using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public int index;
    public int type;//door open, pick, locked door object needed, ui interaction, hide

    public string name;
    public string actionSound;

    [SerializeField] Animator animator;

    //type == 3
    bool opened = false;

    [Header("type = 4")]
    [SerializeField] Vector3 hidePosition;


    System.Action interaction;

    void Awake()
    {
        switch (type)
        {
            case 0:
                interaction = OpenInteraction;
                break;
            case 1:
                interaction = PickInteraction;
                break;
            case 2:
                interaction = OpenWithItemInteraction;
                break;
            case 3:
                interaction = OpenWithUIInteraction;
                break;
            case 4:
                interaction = HideInteraction;
                break;
            default:
                break;
        }
    }

    public void Interact()
    {
        interaction.Invoke();
    }

    void OpenInteraction()
    {
        animator.Play("Interaction");
        DestroyObject();
    }

    void PickInteraction()
    {
        PlayerHands.I.SetItem(index, type);
        DestroyObject();
    }

    void OpenWithItemInteraction()
    {
        if (PlayerHands.I.UseItem(index))
        {
            animator.Play("Interaction");
            DestroyObject();
        }
    }

    void OpenWithUIInteraction()
    {
        InGameUI.I.UIInteraction(this);
    }

    void HideInteraction()
    {
        InGameUI.I.OpenHideInteraction(this);
    }

    //outside method for UI
    public void OpenUIInteraction()
    {
        if (!opened)
        {
            opened = true;
            animator.Play("Interaction");
        }
    }

    void DestroyObject() => Destroy(gameObject);
}
