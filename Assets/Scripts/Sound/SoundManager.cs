using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// May want to see about different enum's for different sounds, otherwise this is going to get fairly large fast.
public enum SoundType
{
    // Movement
    WALK_SAND,
    WALK_WOOD,
    WALK_CONCRETE,
    WALK_METAL,

    // Ambience (not adding yet, it will just be a looping song in the scene)
    BACKGROUND_MUSIC,

    // Interaction
    CLICK,
    POP,
    SUCCESS,
    DING,
    FIREBALL_CAST,
    FIRE_HIT,
    HIT_SOUND,
    KILL_SOUND,
    SHOP_PURCHASE,
    GAME_OVER,
    VICTORY,
    TRANSPORT,
    GATE_OPEN,
    DIM_SWAP,
    SWAP_CHARGE,
    BACKGROUND_OW,
    BACKGROUND_UW
}

public class SoundCollection
{
    private AudioClip[] clips;
    private int lastClipIndex;

    public SoundCollection(params string[] clipNames)
    {
        this.clips = new AudioClip[clipNames.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            clips[i] = Resources.Load<AudioClip>(clipNames[i]);
            if (clips[i] == null)
            {
                Debug.Log($"can't find audio clip {clipNames[i]}");
            }
        }
        lastClipIndex = -1;
    }

    public AudioClip GetRandClip()
    {
        if (clips.Length == 0)
        {
            Debug.Log("No clips to give");
            return null;
        }
        else if (clips.Length == 1)
        {
            return clips[0];
        }
        else
        {
            int index = lastClipIndex;
            while (index == lastClipIndex)
            {
                index = Random.Range(0, clips.Length);
            }
            lastClipIndex = index;
            return clips[index];
        }
    }

}

public class SoundManager : MonoBehaviour
{

    // Logic to manage consistent background music
    private AudioSource _backgroundSpeaker;
    private SoundType? _currentlyPlayingMusic = null;
    public float mainVolume = 1.0f;
    private Dictionary<SoundType, SoundCollection> sounds;
    // private AudioSource audioSrc; Not using a audio source anymore. Making new audio instances to prevent overlap
    public static SoundManager Instance { get; private set; }
    private Coroutine _musicFadeCoroutine; // This is so we can reference and stop the coroutine

    private void Start()
    {
        GameObject bgMusicObj = new GameObject("BackgroundMusic");
        bgMusicObj.transform.SetParent(transform);
        _backgroundSpeaker = bgMusicObj.AddComponent<AudioSource>();
        _backgroundSpeaker.loop = true;
        DontDestroyOnLoad(bgMusicObj);
    }

    public void PlayBackgroundMusic(SoundType musicType, float fadeDuration = 1f)
    {
        if (_currentlyPlayingMusic == musicType)
        {
            return;
        }


        AudioClip clip = GetClip(musicType);
        if (clip != null)
        {
            if (_musicFadeCoroutine != null) // If audio fade is already active, stop it and start again.
            {
                StopCoroutine(_musicFadeCoroutine);
            }

            _musicFadeCoroutine = StartCoroutine(FadeAndSwitchMusic(clip, fadeDuration, musicType));
            _currentlyPlayingMusic = musicType;
        }
        else
        {
        }
    }


    private IEnumerator FadeAndSwitchMusic(AudioClip newClip, float duration, SoundType musicType)
    {
        float startVolume = _backgroundSpeaker.volume;

        // fade out
        float time = 0f;
        while (time < duration)
        {
            _backgroundSpeaker.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _backgroundSpeaker.volume = 0f;

        // swap the clip
        _backgroundSpeaker.Stop();
        _backgroundSpeaker.clip = newClip;

        // bg music needs to start later because of how it starts
        if (musicType == SoundType.BACKGROUND_UW)
        {
            _backgroundSpeaker.time = 2f;
        }

        _backgroundSpeaker.Play();

        // fade in
        time = 0f;
        while (time < duration)
        {
            _backgroundSpeaker.volume = Mathf.Lerp(0f, mainVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _backgroundSpeaker.volume = mainVolume;
    }


    public AudioClip GetClip(SoundType type)
    {
        if (sounds.ContainsKey(type))
        {
            return sounds[type].GetRandClip();
        }
        return null;
    }

    // unity life cycle
    private void Awake()
    {
        Instance = this;
        // audioSrc = GetComponent<AudioSource>();
        sounds = new Dictionary<SoundType, SoundCollection> {
        //  #################################################
        //                     Movement
        //  #################################################
            { SoundType.WALK_CONCRETE, new SoundCollection(
                "Movement/WalkingMaterials/Concrete/Concrete_1",
                "Movement/WalkingMaterials/Concrete/Concrete_2",
                "Movement/WalkingMaterials/Concrete/Concrete_3",
                "Movement/WalkingMaterials/Concrete/Concrete_4"
            )},
            { SoundType.WALK_SAND, new SoundCollection(
                "Movement/WalkingMaterials/Sand/Sand_1",
                "Movement/WalkingMaterials/Sand/Sand_2",
                "Movement/WalkingMaterials/Sand/Sand_3",
                "Movement/WalkingMaterials/Sand/Sand_4"
            )},
            { SoundType.WALK_WOOD, new SoundCollection(
                "Movement/WalkingMaterials/Wood/Wood_1",
                "Movement/WalkingMaterials/Wood/Wood_2",
                "Movement/WalkingMaterials/Wood/Wood_3"
            )},
            { SoundType.WALK_METAL, new SoundCollection(
                "Movement/WalkingMaterials/Metal/Metal_1",
                "Movement/WalkingMaterials/Metal/Metal_2",
                "Movement/WalkingMaterials/Metal/Metal_3"
            )},
        //  #################################################
        //                     INTERACTION
        //  #################################################
            { SoundType.CLICK, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.POP, new SoundCollection(
                "Interaction/Pop"
            )},
            { SoundType.SUCCESS, new SoundCollection(
                "Interaction/Success"
            )},
            { SoundType.DING, new SoundCollection(
                "Interaction/Ding"
            )},
            { SoundType.FIREBALL_CAST, new SoundCollection(
                "Interaction/Fireball"
            )},
            { SoundType.HIT_SOUND, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.KILL_SOUND, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.SHOP_PURCHASE, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.GAME_OVER, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.VICTORY, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.TRANSPORT, new SoundCollection(
                "Interaction/Click"
            )},
            { SoundType.GATE_OPEN, new SoundCollection(
                "Interaction/GATE"
            )},
            { SoundType.FIRE_HIT, new SoundCollection(
                "Interaction/Fire_Hit"
            )},
            { SoundType.DIM_SWAP, new SoundCollection(
                "Interaction/dim_swap"
            )},
            { SoundType.SWAP_CHARGE, new SoundCollection(
                "Interaction/swap_charge"
            )},
            { SoundType.BACKGROUND_OW, new SoundCollection(
                "World_Music/OW_BGM"
            )},
            { SoundType.BACKGROUND_UW, new SoundCollection(
                "World_Music/UW_BGM"
            )},

        };
    }


    public void Play(SoundType type, float pitch = -1f, float volume = 1f)
    {
        if (sounds.ContainsKey(type))
        {
            GameObject parent = GameObject.Find("tempAudio"); // Make a gameObject that all temp objects are made under
            if (parent == null)
            {
                parent = new GameObject("tempAudio");
            }

            GameObject tempAudioObject = new GameObject($"tempAudioObject_{type}");
            tempAudioObject.transform.parent = parent.transform; // set location
            var speaker = tempAudioObject.AddComponent<AudioSource>();

            speaker.volume = Random.Range(0.70f, 1.0f) * mainVolume * volume;
            // Randomizes pitch if it isnt specified.
            if (pitch == -1f)
            {
                speaker.pitch = Random.Range(0.70f, 1.0f);
            }
            else
            {
                speaker.pitch = pitch;
            }

            // Setting to 1 since it needs to sink to the animation better
            speaker.clip = sounds[type].GetRandClip();
            speaker.Play();
            Destroy(tempAudioObject, speaker.clip.length / Mathf.Abs(speaker.pitch)); // Math to only exist for the length of the audio clip
        }
    }
}