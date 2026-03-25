using UnityEngine;

public class Mixing : MonoBehaviour
{
    public Bowl bowl;
    public float minMixSpeed = 0.5f;

    private Spoon currentSpoon;
    private bool spoonInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            Debug.Log("SPOON ENTERED");
            spoonInside = true;
            currentSpoon = other.GetComponent<Spoon>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            spoonInside = false;
            currentSpoon = null;
        }
    }

    private void Update()
    {
        if (spoonInside && currentSpoon != null)
        {
            float speed = currentSpoon.smoothedSpeed;

            if (speed > minMixSpeed)
            {
                float effectiveSpeed = speed - minMixSpeed;
                bowl.Mix(effectiveSpeed);
            }
        }
    }
}