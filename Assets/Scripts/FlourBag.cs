using UnityEngine;

public class FlourBag : MonoBehaviour
{
    private float flourAmount = 100f;

    public float ReduceFlour(float amount)
    {
        float available = flourAmount;

        float taken = Mathf.Min(amount, available);

        flourAmount -= taken;

        return taken;
    }
    private void OnTriggerEnter(Collider other)
    {
        FlourScooping spoon = other.GetComponent<FlourScooping>();

        if (spoon != null)
        {
            spoon.EnterFlour(this.GetComponentInParent<FlourBag>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FlourScooping spoon = other.GetComponent<FlourScooping>();

        if (spoon != null)
        {
            spoon.ExitFlour();
        }
    }
}
