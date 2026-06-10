using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbientLoop : MonoBehaviour
{
    public EventReference fridgeLoop;

    private EventInstance instance;

    public float volume = 1f;
    public float minDistance = 1f;
    public float maxDistance = 15f;

    private void Start()
    {
        instance =
            RuntimeManager.CreateInstance(fridgeLoop);

        instance.set3DAttributes(
            RuntimeUtils.To3DAttributes(transform)
        );

        instance.setProperty(
    FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE,
    minDistance);

        instance.setProperty(
            FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE,
            maxDistance);

        instance.setVolume(volume);

        instance.start();
        Debug.Log("Starting fridge loop");
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