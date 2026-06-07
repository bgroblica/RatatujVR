using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketCollision : MonoBehaviour
{
    private readonly List<(Collider, Collider)> ignoredPairs =
        new List<(Collider, Collider)>();

    // Track what is currently socketed (prevents double-exit issues)
    private readonly HashSet<Cake> socketedCakes = new HashSet<Cake>();

    private void OnEnable()
    {
        RegisterSockets();
    }

    private void OnDisable()
    {
        XRSocketInteractor[] sockets =
            FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketEntered);
            socket.selectExited.RemoveListener(OnSocketExited);
        }
    }

    private void RegisterSockets()
    {
        XRSocketInteractor[] sockets =
            FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketEntered);
            socket.selectExited.RemoveListener(OnSocketExited);

            socket.selectEntered.AddListener(OnSocketEntered);
            socket.selectExited.AddListener(OnSocketExited);
        }

        Debug.Log($"SocketCollision registered to {sockets.Length} sockets");
    }

    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        Cake cake =
            args.interactableObject.transform.GetComponentInParent<Cake>();

        if (cake == null)
            return;

        Transform socketTransform =
            args.interactorObject.transform;

        socketedCakes.Add(cake);

        SetSocketedState(cake.gameObject, true);

        IgnoreWithHierarchy(cake.transform, socketTransform);

        Debug.Log($"{cake.name} socketed");
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        Cake cake =
            args.interactableObject.transform.GetComponentInParent<Cake>();

        if (cake == null)
            return;

        socketedCakes.Remove(cake);

        SetSocketedState(cake.gameObject, false);

        if (socketedCakes.Count == 0)
        {
            RestoreCollisions();
        }

        Debug.Log($"{cake.name} unsocketed");
    }

    private void IgnoreWithHierarchy(Transform cakeRoot, Transform socketRoot)
    {
        RestoreCollisions();

        Collider[] cakeColliders =
            cakeRoot.GetComponentsInChildren<Collider>(true);

        Transform current = socketRoot;

        while (current != null)
        {
            Collider[] socketColliders =
                current.GetComponentsInChildren<Collider>(true);

            foreach (Collider c1 in cakeColliders)
            {
                foreach (Collider c2 in socketColliders)
                {
                    if (c1 == c2)
                        continue;

                    Physics.IgnoreCollision(c1, c2, true);
                    ignoredPairs.Add((c1, c2));
                }
            }

            current = current.parent;
        }
    }

    public void RestoreCollisions()
    {
        foreach (var pair in ignoredPairs)
        {
            if (pair.Item1 && pair.Item2)
            {
                Physics.IgnoreCollision(pair.Item1, pair.Item2, false);
            }
        }

        ignoredPairs.Clear();
    }

    private void SetSocketedState(GameObject obj, bool socketed)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.isKinematic = socketed;

        if (socketed)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}