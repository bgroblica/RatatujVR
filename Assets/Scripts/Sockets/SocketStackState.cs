using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketCollision : MonoBehaviour
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
        if (socketed)
        {
            foreach (var rb in rbs)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
                rb.isKinematic = true; // safe because we disable collisions too
            }

            foreach (var col in cols)
                col.enabled = false;
        }
        else
        {
            foreach (var rb in rbs)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            foreach (var col in cols)
                col.enabled = true;
        }
    }
}