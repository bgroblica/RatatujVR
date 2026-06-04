using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CakeStackReader : MonoBehaviour
{
    public XRSocketInteractor startSocket;

    public class CakeLayerDataRuntime
    {
        public Cake cake;
        public List<GameObject> decorations = new List<GameObject>();
    }

    public List<CakeLayerDataRuntime> GetFullCake()
    {
        List<CakeLayerDataRuntime> result =
            new List<CakeLayerDataRuntime>();

        XRSocketInteractor currentSocket = startSocket;

        while (currentSocket != null && currentSocket.hasSelection)
        {
            Cake cake =
                currentSocket
                .GetOldestInteractableSelected()
                .transform
                .GetComponent<Cake>();

            if (cake == null)
                break;

            CakeLayerDataRuntime layer =
                new CakeLayerDataRuntime();

            layer.cake = cake;

            // READ DECORATIONS
            if (cake.decorationSockets != null)
            {
                foreach (var socket in cake.decorationSockets)
                {
                    if (socket != null && socket.hasSelection)
                    {
                        var decoration =
                            socket.GetOldestInteractableSelected()
                            .transform
                            .gameObject;

                        layer.decorations.Add(decoration);
                    }
                }
            }

            result.Add(layer);

            currentSocket = cake.nextLayerSocket;
        }

        return result;
    }
}