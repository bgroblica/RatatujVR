using UnityEngine;

public class FlourScooping : MonoBehaviour
{
    [SerializeField] private FlourSpoon flourSpoon;

    private FlourBag currentBag;

    [Header("Flour Scoop")]
    public float scoopAmount = 1f;

    [Header("Exit Delay")]
    public float exitDelay = 3f;

    private bool inFlourZone = false;
    private float exitTimer = 0f;

    public void EnterFlour(FlourBag bag)
    {
        currentBag = bag;
        inFlourZone = true;
        exitTimer = 0f;
        CollectFlour();
    }

    public void ExitFlour()
    {
        exitTimer = exitDelay;
    }

    private void Update()
    {
        if (exitTimer > 0f)
        {
            exitTimer -= Time.deltaTime;

            if (exitTimer <= 0f)
            {
                exitTimer = 0f;
                inFlourZone = false;
                currentBag = null;
            }
        }
    }

    public bool IsInFlour()
    {
        return inFlourZone || exitTimer > 0f;
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
}