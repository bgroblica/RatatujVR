using UnityEngine;
using FMODUnity;

public class OneShotPlayer : MonoBehaviour
{
    public EventReference oneShotPlayed;

    public void PlayOneShot()
    {
        AudioManager.Instance.PlaySFX(oneShotPlayed);
        Debug.Log("Playing sound from: " + gameObject.name);
    }
}
