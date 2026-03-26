using UnityEngine;

public class MilkCarton : Pourable
{
    public ParticleSystem milkParticles;

    public override void UpdateAmount()
    {
    }
    protected override void StartPour()
    {
        if (isEmpty) return;

        base.StartPour();

        if (milkParticles != null)
        {
            milkParticles.Play();
        }
    }

    protected override void StopPour()
    {
        base.StopPour();

        if (milkParticles != null)
            milkParticles.Stop();
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
                bowl.AddMilk(amount);
            }
        }
    }
}