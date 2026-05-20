using UnityEngine;

public class EggPourable : Pourable
{
    public ParticleSystem pourParticles;

    public override void UpdateAmount()
    {
        Debug.Log("Egg amount: " + currentAmount);
    }

    protected override void StartPour()
    {
        if (isEmpty || currentAmount <= 0f) return;

        base.StartPour();

        if (pourParticles != null)
            pourParticles.Play();
    }

    protected override void StopPour()
    {
        base.StopPour();

        if (pourParticles != null)
            pourParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    protected override void Pour(float amount)
    {
        if (pourPoint == null) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 2f))
        {
            Bowl bowl = hit.collider.GetComponentInParent<Bowl>();

            if (bowl != null)
                bowl.AddEgg(amount);
        }
    }

    public void Fill()
    {
        currentAmount = maxAmount;
        isEmpty = false;
    }
}