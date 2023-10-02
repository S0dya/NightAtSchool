using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : SingletonMonobehaviour<InGameUI>
{
    [SerializeField] Player player;

    [Header("UI interaction")]
    [SerializeField] CanvasGroup interactionUIWindowCG;
    [SerializeField] GameObject[] UIInteractions;
    [HideInInspector] public int index;
    Interactable curInteractable;

    //local uiInteraction
    int curGuess = 0;

    //lock 3x3 with buttons not interacable after interaction
    [SerializeField] Image[] passwordLock3x3IOAIButtonImages;
    bool[] buttonsPressed = new bool[9];

    [Header("Hide")]
    [SerializeField] CanvasGroup inputUICG;
    [SerializeField] CanvasGroup HideInteractionCG;
    Vector3 lastPlayerPosition;

    protected override void Awake()
    {
        base.Awake();

    }

    public void UIInteraction(Interactable interactable)
    {
        curInteractable = interactable;

        index = curInteractable.index - 50;

        ToggleUIInteraction(true);
    }
    
    public void OpenHideInteraction(Interactable interactable)
    {
        curInteractable = interactable;
        lastPlayerPosition = player.transform.position;

        if (Enemy.I.enemyVision.seesPlayer) ChangeTargetForEnemy();

        player.ToggleHide(true);
        GameManager.I.Open(HideInteractionCG, 0.1f);

        player.transform.position = curInteractable.hideTransform.position;
    }

    //buttons
    public void EnterButton()
    {
        curInteractable.OpenUIInteraction();
    }

    public void ExitButton()
    {
        ToggleUIInteraction(false);
    }

    public void ExitHideButton()
    {
        player.transform.position = lastPlayerPosition;
        GameManager.I.Close(HideInteractionCG, 0.1f);
        player.ToggleHide(false);
    }



    //UIInteractionButtons
    public void Lock3x3IOAIButton(int i)
    {
        if (!buttonsPressed[i])
        {
            ToggleButtonPressed(true, i);

            if (Settings.lockPassword[index][curGuess] == i)
            {
                curGuess++;

                if (curGuess == Settings.lockPassword[index].Length)
                {
                    curInteractable.OpenUIInteraction();
                    ToggleUIInteraction(false);
                }
            }
            else
            {
                curGuess = 0;
            }
        }
    }
    public void ResetLock3x3IOAIButton()
    {
        for (int i = 0; i < 9; i++) ToggleButtonPressed(false, i);
        curGuess = 0;
    }
    void ToggleButtonPressed(bool val, int i)
    {
        buttonsPressed[i] = val;
        passwordLock3x3IOAIButtonImages[i].color = new Color(0, 0, 0, val ? 0.8f : 0.2f);
        passwordLock3x3IOAIButtonImages[i].raycastTarget = !val;
    }


    //otherMethods
    void ToggleUIInteraction(bool val)
    {
        UIInteractions[index].SetActive(val);

        if (val) GameManager.I.Open(interactionUIWindowCG, 0.4f);
        else GameManager.I.Close(interactionUIWindowCG, 0);
    }

    void ChangeTargetForEnemy()
    {
        Enemy.I.ChooseNextTarget(curInteractable.enemyTarget);
        Enemy.I.sawPlayerBeforeHiding = true;
    }

    //UIInteractionMethods

}
