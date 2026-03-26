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

    [Header("Amount")]
    public float maxAmount = 5f;
    [SerializeField] protected float currentAmount;

    protected bool isPouring = false;
    protected bool isHeld = false;
    protected bool isEmpty = false;

    private XRGrabInteractable grabInteractable;

    protected virtual void Awake()
    {
        currentAmount = maxAmount;

        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    protected virtual void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    protected virtual void ReduceAmount(float amount)
    {
        currentAmount -= amount;
        currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);
        UpdateAmount();
    }
    protected virtual float GetCurrentAmount()
    {
        return currentAmount;
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
            StartPour();
        }
        else if (angle <= pourAngle && isPouring)
        {
            StopPour();
        }
    }
    public abstract void UpdateAmount();
    protected virtual void StartPour()
    {
        if (GetCurrentAmount() <= 0f || isEmpty)
        {
            Debug.Log("EMPTY - can't pour");
            return;
        }

        isPouring = true;
        InvokeRepeating(nameof(PourTick), 0f, pourRate);
    }

    protected virtual void StopPour()
    {
        isPouring = false;
        CancelInvoke(nameof(PourTick));
    }

    private void PourTick()
    {
        if (GetCurrentAmount() <= 0f)
        {
            isEmpty = true;
            StopPour();
            return;
        }

        isEmpty = false;

        float amountToPour = pourRate;

        ReduceAmount(amountToPour);

        Pour(amountToPour);
    }

    protected abstract void Pour(float amount);
}