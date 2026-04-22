using UnityEngine;

public class FlourBag : MonoBehaviour
{
    [SerializeField] private Collider flourCollider;

    public float flourAmount = 100f;

    public float ReduceFlour(float amount)
    {
        float available = flourAmount;

        float taken = Mathf.Min(amount, available);

        flourAmount -= taken;

        return taken;
    }
    private void OnTriggerEnter(Collider flourCollider)
    {
        FlourScooping spoon = flourCollider.GetComponent<FlourScooping>();

        if (spoon != null)
        {
            spoon.EnterFlour(this.GetComponentInParent<FlourBag>());
        }
    }

    private void OnTriggerExit(Collider flourCollider)
    {
        FlourScooping spoon = flourCollider.GetComponent<FlourScooping>();

        if (spoon != null)
        {
            spoon.ExitFlour();
        }
    }
}
