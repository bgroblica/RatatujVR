using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    private List<Mold> moldsInside = new List<Mold>();

    public Animator animator;

    private bool isOpen = false;

    private bool isOn = false;

    private void OnTriggerEnter(Collider other)
    {
        Mold mold = other.GetComponentInParent<Mold>();

        if (mold != null)
        {
            if (mold.HasBatter())
            {
                Debug.Log("MOLD WITH BATTER ENTERED");
                moldsInside.Add(mold);
            }
            else
            {
                Debug.Log("EMPTY MOLD - NOT BAKING");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Mold mold = other.GetComponentInParent<Mold>();

        if (mold != null)
        {
            moldsInside.Remove(mold);
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        animator.SetBool("Open", isOpen);
    }

    public void TogglePower()
    {
        isOn = !isOn;

        Debug.Log("Oven power: " + (isOn ? "ON" : "OFF"));
    }

    private void Update()
    {
        if (!isOn || isOpen)
            return;

        foreach (Mold mold in moldsInside)
        {
            if (mold != null && mold.HasBatter())
            {
                mold.Baking();
            }

            if (mold != null && mold.IsFullyBaked())
            {
                mold.ReleaseCake();
            }
        }
    }
}