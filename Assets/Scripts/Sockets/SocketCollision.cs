using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketCollision : MonoBehaviour
{
    private readonly List<(Collider, Collider)> ignoredPairs =
        new List<(Collider, Collider)>();

    private void Start()
    {
        XRSocketInteractor[] sockets =
            FindObjectsByType<XRSocketInteractor>(
                FindObjectsSortMode.None
            );

        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.AddListener(OnSocketEntered);
            socket.selectExited.AddListener(OnSocketExited);
        }
    }

    private void OnDestroy()
    {
        XRSocketInteractor[] sockets =
            FindObjectsByType<XRSocketInteractor>(
                FindObjectsSortMode.None
            );

        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketEntered);
            socket.selectExited.RemoveListener(OnSocketExited);
        }
    }

    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform != transform)
            return;

        Transform socketTransform =
            args.interactorObject.transform;

        IgnoreWithHierarchy(socketTransform);
        Debug.Log(name + " socketed");
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform != transform)
            return;

        RestoreCollisions();
        Debug.Log(name + " unsocketed");
    }

    private void IgnoreWithHierarchy(Transform target)
    {
        RestoreCollisions();

        Collider[] myColliders =
            GetComponentsInChildren<Collider>();

        Transform current = target;

        while (current != null)
        {
            Collider[] targetColliders =
                current.GetComponentsInChildren<Collider>();

            foreach (Collider myCollider in myColliders)
            {
                foreach (Collider targetCollider in targetColliders)
                {
                    if (myCollider == targetCollider)
                        continue;

                    Physics.IgnoreCollision(
                        myCollider,
                        targetCollider,
                        true
                    );

                    ignoredPairs.Add(
                        (myCollider, targetCollider)
                    );
                }
            }

            current = current.parent;
        }
    }

    public void RestoreCollisions()
    {
        foreach (var pair in ignoredPairs)
        {
            if (pair.Item1 != null &&
                pair.Item2 != null)
            {
                Physics.IgnoreCollision(
                    pair.Item1,
                    pair.Item2,
                    false
                );
            }
        }

        ignoredPairs.Clear();
    }
}