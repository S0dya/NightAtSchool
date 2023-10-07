using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public int index;
    public int type;//door open, pick, object needed, ui interaction, hide
    public string nameOfObj;
    public string actionSound;
    public int cutsceneIndex;

    [SerializeField] Animator animator;

    public Transform[] newAreasForEnemy;

    [Header("type == 1")]
    public int indexOfHandObj;

    [Header("type == 2")]
    public int neededInteraction;
    [SerializeField] GameObject[] objectsToAppear;
    int curInteraction;
    
    //type == 3
    bool opened = false;
    
    [Header("type == 4")]
    public Transform hideTransform;
    public Transform enemyTarget;
    
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
        if (cutsceneIndex > 0) TimelineManager.I.PlayCutscene(cutsceneIndex - 1);
    }

    void OpenInteraction()
    {
        AddAreas();
        animator.Play("Interaction");
        DestroyObject();
    }

    void PickInteraction()
    {
        PlayerHands.I.SetItem(this);
        DestroyObject();
    }

    void OpenWithItemInteraction()
    {
        if (PlayerHands.I.UseItem(index))
        {
            if (neededInteraction == 0)
            {
                animator.Play("Interaction");
                DestroyObject();
            }
            else
            {
                //animator.Play($"Interaction{curInteraction}");
                objectsToAppear[curInteraction].SetActive(true);
                curInteraction++;
                if (curInteraction == neededInteraction)
                {
                    animator.Play($"Interaction");
                    DestroyObject();
                }
            }
            AddAreas();
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

    void AddAreas()
    {
        foreach (var newAreaForEnemy in newAreasForEnemy)
        {
            Enemy.I.patrolPoints.Add(newAreaForEnemy);
        }
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
