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
    public int cutsceneIndex;

    [SerializeField] Animator animator;

    public Transform[] newAreasForEnemy;

    [Header("type == 1")]
    public int indexOfHandObj;

    [Header("type == 2")]
    public int neededInteraction;
    [SerializeField] GameObject[] objectsToAppear;
    int curInteraction;
    
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
        Open();
    }

    void PickInteraction()
    {
        PlayerHands.I.SetItem(this);
        PlaySoundPick();
        DestroyObject();
    }

    void OpenWithItemInteraction()
    {
        if (PlayerHands.I.UseItem(index))
        {
            if (neededInteraction == 0) Open();
            else
            {
                objectsToAppear[curInteraction].SetActive(true);
                curInteraction++;
                if (curInteraction == neededInteraction) Open();
            }
        }
    }

    void Open()
    {
        animator.Play("Interaction");
        AddAreas();
    }

    void OpenWithUIInteraction()
    {
        InGameUI.I.UIInteraction(this);
    }

    void HideInteraction()
    {
        InGameUI.I.OpenHideInteraction(this);
    }

    public void AddAreas()
    {
        PlaySoundDoor();
        foreach (var newAreaForEnemy in newAreasForEnemy) Enemy.I.patrolPoints.Add(newAreaForEnemy);
        DestroyObject();
    }

    //outside method for UI
    public void OpenUIInteraction()
    {
        Open();
    }

    void DestroyObject() => Destroy(gameObject);

    void PlaySoundDoor()
    {
        AudioManager.I.PlayOneShot(AudioManager.I.DoorOpen, transform.position);
    }
    void PlaySoundPick()
    {
        AudioManager.I.PlayOneShot(AudioManager.I.ItemPick, transform.position);
    }
}
