using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketCollision : MonoBehaviour
{
    private readonly List<(Collider, Collider)> ignoredPairs = new();

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
        var cake = args.interactableObject.transform.GetComponentInParent<Cake>();
        if (!cake) return;

        Transform socket = args.interactorObject.transform;

        IgnoreBetween(cake.transform, socket);
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        RestoreCollisions();
    }

    private void IgnoreBetween(Transform objRoot, Transform socketRoot)
    {
        RestoreCollisions();

        var objCols = objRoot.GetComponentsInChildren<Collider>(true);
        var socketCols = socketRoot.GetComponentsInChildren<Collider>(true);

        // IMPORTANT: also include parent stacks above socket
        var socketStackCols = socketRoot.root.GetComponentsInChildren<Collider>(true);

        foreach (var c1 in objCols)
        {
            foreach (var c2 in socketStackCols)
            {
                if (c1 == c2) continue;

                Physics.IgnoreCollision(c1, c2, true);
                ignoredPairs.Add((c1, c2));
            }
        }
    }

    private void RestoreCollisions()
    {
        foreach (var pair in ignoredPairs)
        {
            if (pair.Item1 && pair.Item2)
                Physics.IgnoreCollision(pair.Item1, pair.Item2, false);
        }

        ignoredPairs.Clear();
    }
}