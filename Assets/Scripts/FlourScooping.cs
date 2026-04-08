using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlourScooping : MonoBehaviour
{
    public bool isInFlour = false;

    private FlourBag currentBag;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    [Header("Flour")]
    public float flourAmount = 0f;
    public float maxFlour = 1f;
    public float collectSpeed = 0.5f;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }
    public void EnterFlour(FlourBag bag)
    {
        isInFlour = true;
        currentBag = bag;
    }

    public void ExitFlour()
    {
        isInFlour = false;
        currentBag = null;
    }

    private void Update()
    {
        if (isInFlour)
        {
            CollectFlour();
        }
    }

    private void CollectFlour()
    {
        if (flourAmount >= maxFlour) return;

        flourAmount += collectSpeed * Time.deltaTime;
        flourAmount = Mathf.Clamp(flourAmount, 0f, maxFlour);
    }
}