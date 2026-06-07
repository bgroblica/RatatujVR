using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketCollision : MonoBehaviour
{
    private void OnEnable()
    {
        var sockets = FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnSocketEntered);
            socket.selectExited.AddListener(OnSocketExited);
        }
    }

    private void OnDisable()
    {
        var sockets = FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (var socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketEntered);
            socket.selectExited.RemoveListener(OnSocketExited);
        }
    }

    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        var cake = args.interactableObject.transform.GetComponentInParent<Rigidbody>();
        if (!cake) return;

        StartCoroutine(SafeSocketEnter(cake));
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        var rb = args.interactableObject.transform.GetComponentInParent<Rigidbody>();
        if (!rb) return;

        StartCoroutine(SafeSocketExit(rb));
    }

    private IEnumerator SafeSocketEnter(Rigidbody rb)
    {
        // Let XR finish snapping first
        yield return new WaitForFixedUpdate();

        // Kill motion BEFORE physics reacts
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.isKinematic = true;

        Physics.SyncTransforms();
    }

    private IEnumerator SafeSocketExit(Rigidbody rb)
    {
        // wait one physics step so XR detaches cleanly
        yield return new WaitForFixedUpdate();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Physics.SyncTransforms();
    }
}