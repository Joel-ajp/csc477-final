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
    public AudioClip GetClip(SoundType type)
    {
        if (sounds.ContainsKey(type))
        {
            return sounds[type].GetRandClip();
        }
        return null;
    }

    public float mainVolume = 1.0f;
    private Dictionary<SoundType, SoundCollection> sounds;
    private AudioSource audioSrc;

    public static SoundManager Instance { get; private set; }

    // unity life cycle
    private void Awake()
    {
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
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
        };
    }


    public void Play(SoundType type, float pitch = -1f, AudioSource overrideSource = null)
    {
        if (sounds.ContainsKey(type))
        {
            var source = overrideSource ?? audioSrc;
            audioSrc.volume = Random.Range(0.70f, 1.0f) * mainVolume;
            // Randomizes pitch if it isnt specified.
            if (pitch == -1f)
            {
                audioSrc.pitch = Random.Range(0.70f, 1.0f);
            }
            else
            {
                audioSrc.pitch = pitch;
            }

            // Setting to 1 since it needs to sink to the animation better
            audioSrc.clip = sounds[type].GetRandClip();
            audioSrc.Play();
        }
    }
}