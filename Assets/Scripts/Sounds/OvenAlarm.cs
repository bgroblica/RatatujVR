using UnityEngine;
using FMODUnity;
using System.Collections;

public class OvenAlarm : MonoBehaviour
{
    public EventReference fireAlarm;
    public float alarmLength = 18.5f; // duration of your one-shot

    private Coroutine alarmCoroutine;

    public void OvenOn()
    {
        if (alarmCoroutine == null)
            alarmCoroutine = StartCoroutine(PlayAlarm());
    }

    public void OvenOff()
    {
        if (alarmCoroutine != null)
        {
            StopCoroutine(alarmCoroutine);
            alarmCoroutine = null;
        }
    }

    IEnumerator PlayAlarm()
    {
        while (true)
        {
            RuntimeManager.PlayOneShot(fireAlarm);
            yield return new WaitForSeconds(alarmLength);
        }
    }
}