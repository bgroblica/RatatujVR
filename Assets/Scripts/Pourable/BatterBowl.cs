using UnityEngine;

public class BatterBowl : Pourable
{
    public ParticleSystem batterParticles;

    public Bowl bowl;

    public override void UpdateAmount()
    {
        if (bowl != null)
        {
            currentAmount = bowl.GetBatterAmount();
        }
    }
    protected override void StartPour()
    {
        if (isEmpty) return;

        if (batterParticles != null)
        {
            if (!bowl.IsFullyMixed())
            {
                Debug.Log("Not mixed yet!");
                return;  
            }

            base.StartPour();

            batterParticles.Play();
            Debug.Log("StartPouring");
        }
    }

    protected override void StopPour()
    {
        base.StopPour();

        if (batterParticles != null)
            batterParticles.Stop();
    }
    protected override float GetCurrentAmount()
    {
        if (bowl == null) return 0f;

        return bowl.milkAmount + bowl.flourAmount;
    }

    protected override void ReduceAmount(float amount)
    {
        if (bowl == null) return;

        float total = bowl.milkAmount + bowl.flourAmount;
        if (total <= 0f) return;

        float milkRatio = bowl.milkAmount / total;
        float flourRatio = bowl.flourAmount / total;

        bowl.milkAmount -= amount * milkRatio;
        bowl.flourAmount -= amount * flourRatio;

        bowl.milkAmount = Mathf.Clamp(bowl.milkAmount, 0f, bowl.maxIngredients);
        bowl.flourAmount = Mathf.Clamp(bowl.flourAmount, 0f, bowl.maxIngredients);

        bowl.UpdateVisualMilk();
        bowl.Mix(0f);
        UpdateAmount();
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
                mold.AddBatter(amount);
            }
        }
    }
}