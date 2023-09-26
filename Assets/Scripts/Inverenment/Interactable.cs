using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public int index;
    public int type;//door open, pick, locked door object needed, ui interaction

    public string name;
    public string actionSound;

    [SerializeField] Animator animator;

    [Header("type = 3")]
    public CanvasGroup cg;
    bool opened = false;


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
