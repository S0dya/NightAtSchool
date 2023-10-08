using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : SingletonMonobehaviour<MenuUI>
{


    protected override void Awake()
    {
        base.Awake();

    }

    //buttons
    public void PlayButton()
    {
        LoadingSceneManager.I.LoadGame();
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
