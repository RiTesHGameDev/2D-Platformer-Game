using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public bool IsMute = false;
    public float Volume = 1.0f;
    public AudioSource SoundEffect;
    public AudioSource SoundMusic;
    public AudioSource SoundFootStep;
    public SoundType[] Audios;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayBGM(Sounds.BackgroundMusic);
    }
    public void Mute(bool status)
    {
        IsMute = status;
    }
    public void SetVolume(float volume)
    {
        Volume = volume;
        SoundMusic.volume = volume;
        SoundEffect.volume = volume;
    }
    public void PlayBGM(Sounds sound)
    {
        if (IsMute) 
        return;

        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SoundMusic.clip = clip;
            SoundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound type !" + sound);
        }

    }
    // Stop the background music
    public void Stop(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SoundFootStep.clip = clip;
            SoundFootStep.Stop();
        }
        else
        {
            Debug.LogError("Clip not found for sound type !" + sound);
        }
    }
    public void PlayFootStep(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SoundFootStep.clip = clip;
            SoundFootStep.Play();
            //SoundFootStep.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("FootStep Clip not found for sound type !" + sound);
        }

    }
    public void Play(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SoundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound type !" + sound);
        }

    }
    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Audios,i =>i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }
}
[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}
public enum Sounds
{
    BackgroundMusic,
    ButtonClick,
    UnlockedButtonClick,
    Key,
    levelComplete,
    PlayerJump,
    PlayerMove,
    PlayerDead,
    ReduceLife,
    EnemyDead,
}