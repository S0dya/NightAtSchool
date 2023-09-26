using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : SingletonMonobehaviour<InGameUI>
{
    //uiInteraction
    [SerializeField] GameObject[] UIInteractions;
    [HideInInspector] public int index;
    Interactable curInteractable;


    protected override void Awake()
    {
        base.Awake();

    }

    public void UIInteraction(Interactable interactable)
    {
        curInteractable = interactable;

        index = curInteractable.index;
        GameManager.I.Open(curInteractable.cg, 0.2f);

        ToggleUIInteraction(true);
    }

    //buttons
    public void EnterButton()
    {
        curInteractable.OpenUIInteraction();
    }

    public void ExitButton()
    {
        GameManager.I.Close(curInteractable.cg, 0.1f);
        ToggleUIInteraction(true);
    }

    //otherMethods
    void ToggleUIInteraction(bool val)
    {
        UIInteractions[index].SetActive(val);
    }
}
