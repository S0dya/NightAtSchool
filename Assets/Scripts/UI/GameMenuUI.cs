using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : SingletonMonobehaviour<GameMenuUI>
{
    [SerializeField] CanvasGroup gameMenuCG;

    [SerializeField] GameObject resumeButtonObj;
    [SerializeField] GameObject replayButtonObj;

    [SerializeField] TextMeshProUGUI headText;

    [SerializeField] Image backgroundImage;

    protected override void Awake()
    {
        base.Awake();

    }

    //buttons
    public void ResumeButton()
    {
        CloseMenu();
    }
    public void ExitMenuButton()
    {
        LoadingSceneManager.I.LoadMenu();
    }

    public void NewGameButton()
    {
        LoadingSceneManager.I.RestartGame();
    }

    public void PlayButtonSound()
    {
        AudioManager.I.PlayButtonSound();
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
        AudioManager.I.EventInstancesDict["Ambience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
