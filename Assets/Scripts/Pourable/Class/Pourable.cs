using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public abstract class Pourable : MonoBehaviour
{
    [Header("Pour Settings")]
    public Transform pourPoint;
    public float pourAngle = 60f;
    public float pourRate = 0.02f;

    [Header("Amount")]
    public float maxAmount = 5f;
    [SerializeField] protected float currentAmount;

    protected bool isPouring = false;
    protected bool isHeld = false;
    protected bool isEmpty = false;

    private XRGrabInteractable grabInteractable;

    public bool IsPouring => isPouring;

    protected virtual void Awake()
    {
        currentAmount = maxAmount;

        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    protected virtual void OnDestroy()
    {
        if (grabInteractable == null) return;

        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    public virtual float GetCurrentAmount()
    {
        return currentAmount;
    }

    protected virtual void ReduceAmount(float amount)
    {
        currentAmount -= amount;
        currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);
        UpdateAmount();
    }

    protected virtual void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    protected virtual void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        StopPour();
    }

    protected virtual void Update()
    {
        if (!isHeld) return;

        float angle = Vector3.Angle(transform.up, Vector3.up);

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
    protected abstract void Pour(float amount);

    protected virtual void StartPour()
    {
        if (GetCurrentAmount() <= 0f || isEmpty)
            return;

        isPouring = true;

        OnStartPour();

        InvokeRepeating(nameof(PourTick), 0f, pourRate);
    }

    protected virtual void StopPour()
    {
        isPouring = false;

        OnStopPour();

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

    // 🔥 NEW HOOKS (key fix)
    protected virtual void OnStartPour() { }
    protected virtual void OnStopPour() { }
}