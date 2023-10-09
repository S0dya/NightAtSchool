using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    bool calmMusicIsCurrentlyPlaying;

    Coroutine fadeOutCoroutine;
    Coroutine randomSFXCor;

    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Music { get; private set; }

    [field: SerializeField] public EventReference PlayerSteps { get; private set; }

    [field: SerializeField] public EventReference EnemySteps { get; private set; }
    [field: SerializeField] public EventReference EnemyBreath { get; private set; }
    [field: SerializeField] public EventReference EnemySawPlayer { get; private set; }
    [field: SerializeField] public EventReference EnemyLostPlayer { get; private set; }

    [field: SerializeField] public EventReference ItemPick { get; private set; }
    [field: SerializeField] public EventReference KeyDrop { get; private set; }
    [field: SerializeField] public EventReference BatteryDrop { get; private set; }
    [field: SerializeField] public EventReference DoorOpen { get; private set; }

    [field: SerializeField] public EventReference Jumpscare { get; private set; }

    [field: SerializeField] public EventReference ButtonPress { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();
    }

    void Start()
    {
        EventInstancesDict.Add("Ambience", CreateInstance(Ambience));
        EventInstancesDict.Add("Music", CreateInstance(Music));

        EventInstancesDict.Add("PlayerSteps", CreateInstance(PlayerSteps));

        EventInstancesDict.Add("EnemySawPlayer", CreateInstance(EnemySawPlayer));
        EventInstancesDict.Add("EnemyLostPlayer", CreateInstance(EnemyLostPlayer));
        EventInstancesDict.Add("Jumpscare", CreateInstance(Jumpscare));

        EventInstancesDict.Add("ButtonPress", CreateInstance(ButtonPress));

        EventInstancesDict["Music"].start();

        for (int i = 0; i < Settings.soundVolume.Length; i++)
        {
            ChangeVolume(i, Settings.soundVolume[i]);
        }
    }

    public void SetParameter(string instanceName, string parameterName, float value)
    {
        EventInstancesDict[instanceName].setParameterByName(parameterName, value);
    }
    public void SetParameterWithCheck(string instanceName, string parameterName, float newValue)
    {
        float currentParameterValue;
        EventInstancesDict[parameterName].getParameterByName(parameterName, out currentParameterValue);

        if (currentParameterValue != newValue)
        {
            EventInstancesDict[parameterName].setParameterByName(parameterName, newValue);
        }
    }


    public void PlayOneShot(string sound)
    {
        EventInstancesDict[sound].start();
    }
    public void PlayOneShot(EventReference sound, Vector3 pos)
    {
        RuntimeManager.PlayOneShot(sound, pos);
    }

    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }


    public void ChangeVolume(int i, float volume)
    {
        Settings.soundVolume[i] = volume;

        string bus = (i == 0 ? "bus:/Music" : i == 1 ? "bus:/SFX" : "bus:/Ambience");
        RuntimeManager.GetBus(bus).setVolume(volume);
    }

    public void ToggleSound(bool val)
    {
        RuntimeManager.GetBus("bus:/").setVolume(val ? 1 : 0);
    }

    public void PlayButtonSound()
    {
        EventInstancesDict["ButtonPress"].start();
    }
}