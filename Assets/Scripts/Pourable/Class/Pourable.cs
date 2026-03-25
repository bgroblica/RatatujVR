using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public abstract class Pourable : MonoBehaviour
{
    [Header("Pour Settings")]
    public Transform pourPoint;
    public float pourAngle = 60f;
    public float pourRate = 0.02f;
    public float pourDistance = 1f;

    protected bool isPouring = false;
    protected bool isHeld = false;

    private XRGrabInteractable grabInteractable;

    protected virtual void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    protected virtual void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        StopPour();
    }

    protected virtual void Update()
    {
        if (!isHeld) return;

        float angle = Vector3.Angle(transform.forward, Vector3.up);

        if (angle > pourAngle && !isPouring)
        {
            Debug.Log("START POUR");
            StartPour();
        }
        else if (angle <= pourAngle && isPouring)
        {
            Debug.Log("STOP POUR");
            StopPour();
        }
    }

    protected virtual void StartPour()
    {
        isPouring = true;
        InvokeRepeating(nameof(Pour), 0f, pourRate);
    }

    protected virtual void StopPour()
    {
        isPouring = false;
        CancelInvoke(nameof(Pour));
    }

    protected abstract void Pour();
}