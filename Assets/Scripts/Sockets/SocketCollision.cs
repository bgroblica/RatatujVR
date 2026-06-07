using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketStackState : MonoBehaviour
{
    private Rigidbody[] rbs;
    private Collider[] cols;
    private XRGrabInteractable grab;

    private void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>(true);
        cols = GetComponentsInChildren<Collider>(true);
        grab = GetComponent<XRGrabInteractable>();
    }

    public void SetSocketed(bool socketed)
    {
        foreach (var rb in rbs)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = !socketed;
            rb.isKinematic = socketed; // safe because colliders are disabled in socketed state
        }

        foreach (var col in cols)
        {
            col.enabled = !socketed;
        }

        // IMPORTANT: XR must always stay enabled
        if (grab != null)
            grab.enabled = true;
    }
}