using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class LoopPlayer : MonoBehaviour
{
    public EventReference mixerLoop;

    private EventInstance mixerInstance;

    private void Start()
    {
        mixerInstance =
            RuntimeManager.CreateInstance(mixerLoop);
    }

    public void StartMixing()
    {
        mixerInstance.start();
    }

    public void StopMixing()
    {
        mixerInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        mixerInstance.release();
    }
}