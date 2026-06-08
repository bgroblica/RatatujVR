using UnityEngine;

public class Lever : MonoBehaviour
{
    public Animator animator;

    public void ToggleLever()
    {


        animator.SetTrigger("Lever");
    }
}
