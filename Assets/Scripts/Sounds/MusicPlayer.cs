using UnityEngine;
using FMODUnity;

public class MusicPlayer : MonoBehaviour
{
    public EventReference musicEvent;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(musicEvent);
    }
}