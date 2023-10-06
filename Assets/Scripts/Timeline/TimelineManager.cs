using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimelineManager : SingletonMonobehaviour<TimelineManager>
{
    [SerializeField] PlayableDirector director;
    [SerializeField] PlayableAsset[] cutscenes; //show enemy, game ending

    protected override void Awake()
    {
        base.Awake();

    }

    public void PlayCutscene(int i)
    {
        director.playableAsset = cutscenes[i];
        director.Play();
    }
}
