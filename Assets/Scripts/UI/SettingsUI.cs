using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] CanvasGroup settingsCG;
    [SerializeField] Slider[] soundSliders;

    void Awake()
    {
        for (int i = 0; i < soundSliders.Length; i++)
        {
            soundSliders[i].value = Settings.soundVolume[i];
        }
    }

    //buttons
    public void OpenSettingsButton()
    {
        GameManager.I.Open(settingsCG, 0f);
    }
    public void CloseSettingsButton()
    {
        GameManager.I.Close(settingsCG, 0.1f);
    }

    public void ChangeVolumeButton(int i)
    {
        AudioManager.I.ChangeVolume(i, soundSliders[i].value);
    }
}
