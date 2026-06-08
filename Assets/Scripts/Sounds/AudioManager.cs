using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private EventInstance musicInstance;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(EventReference sound)
    {
        EventInstance instance =
            RuntimeManager.CreateInstance(sound);

        instance.setVolume(sfxVolume);
        instance.start();
        instance.release();
    }

    public void PlayMusic(EventReference music)
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }

        musicInstance =
            RuntimeManager.CreateInstance(music);

        musicInstance.setVolume(musicVolume);
        musicInstance.start();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        if (musicInstance.isValid())
        {
            musicInstance.setVolume(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}