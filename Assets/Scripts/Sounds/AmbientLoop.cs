using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbientLoop : MonoBehaviour
{
    public EventReference fridgeLoop;

    private EventInstance instance;

    private void Start()
    {
        instance =
            RuntimeManager.CreateInstance(fridgeLoop);

        instance.set3DAttributes(
            RuntimeUtils.To3DAttributes(gameObject)
        );

        instance.start();
    }

    private void OnDestroy()
    {
        if (instance.isValid())
        {
            instance.stop(
                FMOD.Studio.STOP_MODE.ALLOWFADEOUT
            );

            instance.release();
        }
    }
}