using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlaceableBlock : MonoBehaviour
{
    public SnapAnchor anchor;
    public float snapDistance = 0.15f;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        TrySnap();
    }

    private void TrySnap()
    {
        SnapSocket[] sockets =
            FindObjectsByType<SnapSocket>(FindObjectsSortMode.None);

        SnapSocket closest = null;
        float closestDist = snapDistance;

        foreach (var socket in sockets)
        {
            if (!socket.IsFree())
                continue;

            if (socket.socketType != anchor.anchorType)
                continue;

            float dist = Vector3.Distance(
                anchor.transform.position,
                socket.transform.position
            );

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = socket;
            }
        }

        if (closest != null)
        {
            SnapTo(closest);
        }
    }

    private void SnapTo(SnapSocket socket)
    {
        // IMPORTANT: stop physics
        rb.isKinematic = true;
        rb.useGravity = false;

        // move EXACTLY to socket
        transform.position = socket.transform.position;
        transform.rotation = socket.transform.rotation;

        // mark occupancy
        socket.Occupy(transform);

        Debug.Log("SNAPPED to " + socket.name);
    }
}