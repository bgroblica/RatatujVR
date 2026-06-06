using UnityEngine;

public class Fridge : MonoBehaviour
{
    public Animator animator;

    private bool isOpen = false;

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        animator.SetBool(
            "Open",
            isOpen
        );
    }
}
