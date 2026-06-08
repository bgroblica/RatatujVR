using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    private List<Mold> moldsInside =
        new List<Mold>();

    public Animator animator;

    public OneShotPlayer ovenButton;
    public OneShotPlayer ovenOpenClose;

    public LoopPlayer looplayer;


    private bool isOpen = false;

    public bool isOn = false;

    private void OnTriggerEnter(Collider other)
    {
        Mold mold =
            other.GetComponentInParent<Mold>();

        if (mold != null)
        {
            if (mold.HasBatter())
            {
                Debug.Log(
                    "MOLD WITH BATTER ENTERED"
                );

                moldsInside.Add(mold);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Mold mold =
            other.GetComponentInParent<Mold>();

        if (mold != null)
        {

            GameObject cakeObject =
            mold.GetCake();

            if (cakeObject != null)
            {
                Cake cake = cakeObject.GetComponent<Cake>();

                if (cake != null)
                {
                    cake.StopOvenAlarm();
                }
            }

            moldsInside.Remove(mold);

            mold.ReleaseCake();


        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        ovenButton.PlayOneShot();

        animator.SetBool(
            "Open",
            isOpen
        );
        ovenOpenClose.PlayOneShot();

    }

    public void TogglePower()
    {
        isOn = !isOn;

        ovenButton.PlayOneShot();

        Debug.Log(
            "Oven Power: " +
            (isOn ? "ON" : "OFF")
        );

        if (isOn == true)
        {
            looplayer.StartMixing();
        }
        else {
            looplayer.StopMixing();
        }

    }

    private void Update()
    {
        if (!isOn)
            return;

        if (isOpen)
            return;

        foreach (Mold mold in moldsInside)
        {
            if (mold == null)
                continue;

            GameObject cakeObject =
                mold.GetCake();

            if (cakeObject == null)
                continue;

            Cake cake =
                cakeObject.GetComponent<Cake>();

            if (cake != null)
            {
                cake.Bake();
            }
        }
    }
}