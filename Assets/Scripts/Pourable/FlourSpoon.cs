using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlourSpoon : Pourable
{
    [SerializeField] private FlourScooping flourScooping;

    [Header("Flour Visuals")]
    public GameObject filledMesh;

    public ParticleSystem flourParticles;

    public override void UpdateAmount()
    {
        // not used
    }

    private void Start()
    {
        RefreshVisual();
    }

    protected override void StartPour()
    {
        if (isEmpty) return;

        if (flourScooping != null && flourScooping.IsInFlour())
        {
            return;
        }

        base.StartPour();

        if (flourParticles != null)
        {
            flourParticles.Play();
        }
    }

    public void StopScoopingPour()
    {
        StopPour();
    }

    public void AddScoopedFlour(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);

        isEmpty = false;

        RefreshVisual();

        Debug.Log("Spoon flour: " + currentAmount);
    }

    protected override void StopPour()
    {
        base.StopPour();

        if (flourParticles != null)
            flourParticles.Stop();
    }

    public bool IsFull()
    {
        return currentAmount >= maxAmount;
    }

    protected override void ReduceAmount(float amount)
    {
        base.ReduceAmount(amount);

        RefreshVisual();
    }

    protected override void Pour(float amount)
    {
        if (pourPoint == null) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 2.0f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Bowl bowl = hit.collider.GetComponentInParent<Bowl>();

            if (bowl != null)
            {
                Debug.Log("HIT BOWL!");
                bowl.AddFlour(amount);
            }
        }
    }

    private void RefreshVisual()
    {
        bool hasFlour = currentAmount > 0f;

        if (filledMesh != null)
            filledMesh.SetActive(hasFlour);
    }
}