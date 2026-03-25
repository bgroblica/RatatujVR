using UnityEngine;

public class MilkCarton : Pourable
{
    public ParticleSystem milkParticles;

    protected override void StartPour()
    {
        base.StartPour();

        if (milkParticles != null)
        {
            milkParticles.Play();
            Debug.Log("StartPouring");
        }

    }

    protected override void StopPour()
    {
        base.StopPour();

        if (milkParticles != null)
            milkParticles.Stop();
    }

    protected override void Pour()
    {
        if (pourPoint == null) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 2.0f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Bowl bowl = hit.collider.GetComponentInParent<Bowl>();

            if (bowl != null)
            {
                Debug.Log("HIT BOWL!");
                bowl.AddMilk(0.1f);
            }
        }
    }
}