using UnityEngine;
using static Cake;

public class IcingPen : MonoBehaviour
{
    public IcingType icingType;

    public LoopPlayer loopPlayer;

    private Cake currentCake;

    private void OnTriggerEnter(Collider other)
    {
        Cake cake =
            other.GetComponentInParent<Cake>();

        if (cake == null)
            return;

        currentCake = cake;

        loopPlayer.StartMixing();

        cake.StartIcing(icingType);
    }

    private void OnTriggerExit(Collider other)
    {
        Cake cake =
            other.GetComponentInParent<Cake>();

        if (cake == null)
            return;

        if (cake != currentCake)
            return;

        loopPlayer.StopMixing();

        cake.StopIcing();

        currentCake = null;
    }
}