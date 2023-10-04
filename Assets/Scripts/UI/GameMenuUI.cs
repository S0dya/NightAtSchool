using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : SingletonMonobehaviour<GameMenuUI>
{
    [SerializeField] CanvasGroup gameMenuCG;
    [SerializeField] CanvasGroup settingsCG;

    [SerializeField] GameObject resumeButtonObj;
    [SerializeField] GameObject replayButtonObj;

    [SerializeField] TextMeshProUGUI headText;

    [SerializeField] Image backgroundImage;

    [Header("Settings")]
    [SerializeField] Slider[] soundSliders;


    protected override void Awake()
    {
        base.Awake();

    }


    //buttons
    public void ResumeButton()
    {
        CloseMenu();
    }
    public void SettingsButton()
    {
        ToggleCG(settingsCG, true);
    }
    public void ExitMenuButton()
    {
        //open Menu
    }

    public void ExitSettingsButton()
    {
        ToggleCG(settingsCG, false);
    }

    public void NewGameButton()
    {
        //new game

    }

    public void ChangeVolumeButton(int i)
    {
        AudioManager.I.ChangeVolume(i, soundSliders[i].value);
    }

    //public methods
    public void OpenGameMenu()
    {
        headText.text = "PAUSE";
        resumeButtonObj.SetActive(true);
        ToggleCG(gameMenuCG, true);

    }
    public void GameOver()
    {
        backgroundImage.enabled = true;
        headText.text = "GAMEOVER";
        replayButtonObj.SetActive(true);
        ToggleCG(gameMenuCG, true);
    }
    

    //other methods
    void CloseMenu()
    {
        ToggleCG(gameMenuCG, false);
        resumeButtonObj.SetActive(false);
        replayButtonObj.SetActive(false);
    }

    void ToggleCG(CanvasGroup CG, bool val)
    {
        if (val) GameManager.I.Open(CG, 0f);
        else GameManager.I.Close(CG, 0);
    }
}
