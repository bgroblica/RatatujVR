using UnityEngine;

public class EggPourable : Pourable
{
    public ParticleSystem pourParticles;

    [Header("Egg Safety")]
    public float pourDelay = 1.5f;

    private bool canPour = false;
    private void EnablePouring()
    {
        canPour = true;
    }

    protected override void Awake()
    {
        base.Awake();

        if (pourParticles == null)
            pourParticles = GetComponentInChildren<ParticleSystem>(true);

        currentAmount = 0f;
        isEmpty = true;
    }

    private void OnEnable()
    {
        canPour = false;

        CancelInvoke(nameof(EnablePouring));
        Invoke(nameof(EnablePouring), pourDelay);
    }

    public void Fill()
    {
        currentAmount = maxAmount;
        isEmpty = false;
    }

    public override void UpdateAmount()
    {
        // optional debug or UI
        Debug.Log("Egg amount" + currentAmount);
    }

    protected override void StartPour()
    {
        if (!canPour) return; 

        base.StartPour();
        if (pourParticles != null)
            pourParticles.Play();
    }

    protected override void OnStopPour()
    {
        if (pourParticles != null)
            pourParticles.Stop();
    }

    protected override void Pour(float amount)
    {
        if (pourPoint == null) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 2f))
        {
            Bowl bowl = hit.collider.GetComponentInParent<Bowl>();

            if (bowl != null)
            {
                bowl.AddEgg(amount);
            }
        }
    }
}