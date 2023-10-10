using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] CanvasGroup CG;

    void Start()
    {
        GameManager.I.Open(CG, 0);
        videoPlayer.loopPointReached += OnVideoStopped;
        videoPlayer.Play();
    }
    void OnVideoStopped(VideoPlayer vp)
    {
        GameManager.I.Close(CG, 0.1f);
        LoadingSceneManager.I.StartGame();
    }
}
