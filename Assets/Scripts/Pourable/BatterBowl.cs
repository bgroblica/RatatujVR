using UnityEngine;

public class BatterBowl : Pourable
{
    public ParticleSystem batterParticles;

    public Bowl bowl;

    public void SetFilled()
    {
        isEmpty = false;
    }

    public override void UpdateAmount()
    {
        if (bowl != null)
        {
            currentAmount = bowl.batterAmount;
        }
    }
    protected override void StartPour()
    {
        if (isEmpty) return;

        if (!bowl.IsFullyMixed())
        {
            Debug.Log("Not mixed yet!");
            return;
        }

        base.StartPour();

        if (batterParticles != null)
        {
            batterParticles.Play();
        }
    }

    protected override void StopPour()
    {
        base.StopPour();

        if (batterParticles != null)
            batterParticles.Stop();
    }

    protected override void ReduceAmount(float amount)
    {
        if (bowl == null) return;

        bowl.batterAmount -= amount;

        bowl.batterAmount =
            Mathf.Clamp(bowl.batterAmount, 0f, bowl.maxIngredients);

        currentAmount = bowl.batterAmount;

        if (bowl.batterAmount <= 0.01f)
        {
            bowl.ResetBowl();
            isEmpty = true;
        }
    }

    protected override void Pour(float amount)
    {
        if (pourPoint == null) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 2.0f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Mold mold = hit.collider.GetComponentInParent<Mold>();

            if (mold != null)
            {
                Debug.Log("HIT BOWL!");
                mold.AddBatter(
                               amount,
                               bowl.batterMilk,
                               bowl.batterFlour,
                               bowl.batterEgg,
                               bowl.batterSugar,
                               bowl.batterButter
                              );
            }
        }
    }
}