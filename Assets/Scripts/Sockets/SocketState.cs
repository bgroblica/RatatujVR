using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketState : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    private const string CAKE_LAYER = "Cake";
    private const string SOCKETED_LAYER = "SocketedCake";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
    }

    public void OnSocketEnter()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void OnSocketExit()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void SetLayerRecursively(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layer;
        }
    }
}