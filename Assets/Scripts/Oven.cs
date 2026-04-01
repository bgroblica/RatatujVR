using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    private List<Mold> moldsInside = new List<Mold>();

    public Animator animator;

    private bool isOpen = false;

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

        Debug.Log("Pressed, hand inside");

        animator.SetBool("Open", isOpen);
    }

    private void Update()
    {
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