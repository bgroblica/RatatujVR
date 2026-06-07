using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobalSocketAutoHook : MonoBehaviour
{
    private void OnEnable()
    {
        var sockets = FindObjectsByType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnSocketEnter);
            socket.selectExited.AddListener(OnSocketExit);
        }
    }

    private void OnDisable()
    {
        var sockets = FindObjectsByType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>(FindObjectsSortMode.None);

        foreach (var socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnSocketEnter);
            socket.selectExited.RemoveListener(OnSocketExit);
        }
    }

    private void OnSocketEnter(SelectEnterEventArgs args)
    {
        var state = args.interactableObject.transform.GetComponentInParent<SocketStackState>();
        if (state != null)
            state.SetSocketed(true);
    }

    private void OnSocketExit(SelectExitEventArgs args)
    {
        var state = args.interactableObject.transform.GetComponentInParent<SocketStackState>();
        if (state != null)
            state.SetSocketed(false);
    }
}