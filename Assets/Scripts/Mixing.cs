using UnityEngine;

public class Mixing : MonoBehaviour
{
    public Bowl bowl;
    public float minMixSpeed = 0.5f;

    private Spoon currentSpoon;
    private bool spoonInside = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER: " + other.name + " | root: " + other.transform.root.name);
        if (other.CompareTag("Spoon"))
        {
            Debug.Log("SPOON ENTERED");
            spoonInside = true;
            currentSpoon = other.GetComponent<Spoon>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT: " + other.name + " | root: " + other.transform.root.name);
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
            if (bowl.IsFullyMixed())
                return;

            float speed = currentSpoon.smoothedSpeed;

            if (speed > minMixSpeed)
            {
                float effectiveSpeed = speed - minMixSpeed;
                bowl.Mix(effectiveSpeed);
            }
        }
    }
}