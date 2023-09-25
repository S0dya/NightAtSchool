using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public int index;
    public int type;

    public string name;
    public string actionSound;

    [SerializeField] Animator animator;

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
    }

    void PickInteraction()
    {
        PlayerHands.I.SetItem(index, type);
    }

    void OpenWithItemInteraction()
    {
        if (PlayerHands.I.UseItem(index))
        {
            animator.Play("Interaction");
        }
    }
}
