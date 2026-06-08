using UnityEngine;

public class FlavourExtract : Pourable
{
    public ParticleSystem flavourParticles;

    public Cake.CakeFlavour flavour;
    public Color flavourColor;

    protected override void StartPour()
    {
        Debug.Log("EXTRACT START");

        base.StartPour();

        if (flavourParticles != null)
            flavourParticles.Play();
    }

    protected override void StopPour()
    {
        Debug.Log("EXTRACT STOP");

        base.StopPour();

        if (flavourParticles != null)
            flavourParticles.Stop();
    }

    protected override void ReduceAmount(float amount)
    {
        // Optional:
        // Leave empty if extract bottles never run out.
    }

    protected override void Pour(float amount)
    {
        if (pourPoint == null)
            return;

        if (Physics.Raycast(
            pourPoint.position,
            Vector3.down,
            out RaycastHit hit,
            2f))
        {
            Bowl bowl =
                hit.collider.GetComponentInParent<Bowl>();

            if (bowl == null)
                return;

            bowl.SetFlavour(flavour);
            bowl.SetFlavourColor(flavourColor);
        }
    }

    public override void UpdateAmount()
    {
    }
}