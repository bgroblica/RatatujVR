using UnityEngine;

public class FlourScooping : MonoBehaviour
{
    [SerializeField] private FlourSpoon flourSpoon;

    private FlourBag currentBag;

    [Header("Flour Scoop")]
    public float scoopAmount = 1f;

    public void EnterFlour(FlourBag bag)
    {
        currentBag = bag;

        flourSpoon.StopScoopingPour();

        CollectFlour();
    }

    public void ExitFlour()
    {
        currentBag = null;
    }

    private void CollectFlour()
    {
        if (currentBag == null) return;

        if (flourSpoon.IsFull())
        {
            Debug.Log("Spoon full");
            return;
        }

        if (currentBag.flourAmount <= 0f)
            return;

        float roomLeft =
            flourSpoon.maxAmount - flourSpoon.GetCurrentAmount();

        float taken = Mathf.Min(
            scoopAmount,
            currentBag.flourAmount,
            roomLeft
        );

        currentBag.ReduceFlour(taken);

        flourSpoon.AddScoopedFlour(taken);

        Debug.Log("Collected flour: " + taken);
    }

    public bool IsInFlour()
    {
        return currentBag != null;
    }
}