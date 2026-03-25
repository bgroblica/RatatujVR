using UnityEngine;

public class FlourBag : Pourable
{
    public ParticleSystem flourParticles;

    protected override void StartPour()
    {
        base.StartPour();

        if (flourParticles != null)
        {
            flourParticles.Play();
            Debug.Log("StartPouring");
        }

    }

    protected override void StopPour()
    {
        base.StopPour();

        if (flourParticles != null)
            flourParticles.Stop();
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
                bowl.AddFlour(0.1f);
            }
        }
    }
}