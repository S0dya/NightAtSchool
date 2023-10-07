using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScreamerUI : SingletonMonobehaviour<ScreamerUI>
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] CanvasGroup CG;

    protected override void Awake()
    {
        base.Awake();

    }

    public void PlayScreamer()
    {
        Destroy(Enemy.I.gameObject);
        GameManager.I.Open(CG, 0);
        videoPlayer.loopPointReached += OnVideoStopped;
        videoPlayer.Play();
    }
    void OnVideoStopped(VideoPlayer vp)
    {
        GameManager.I.Close(CG, 0.1f);
        Player.I.Die();
    }

    
}
