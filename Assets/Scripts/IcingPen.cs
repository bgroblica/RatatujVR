using UnityEngine;
using static Cake;

public class IcingPen : MonoBehaviour
{
    public IcingType icingType;
    public float applyTime = 2f;

    private float contactTime = 0f;
    private Cake currentCake;
    private bool isTouching = false;

    private void OnTriggerEnter(Collider other)
    {
        Cake cake = other.GetComponentInParent<Cake>();

        if (cake == null)
            return;

        Debug.Log("Cake inside");

        currentCake = cake;
        isTouching = true;
        contactTime = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        Cake cake = other.GetComponentInParent<Cake>();

        Debug.Log("Cake left");

        if (cake == currentCake)
        {
            isTouching = false;
            contactTime = 0f;
            currentCake = null;
        }
    }

    private void Update()
    {
        if (!isTouching || currentCake == null)
            return;

        contactTime += Time.deltaTime;

        Debug.Log("Icing...");

        if (contactTime >= applyTime)
        {
            ApplyIcing();
        }
    }

    private void ApplyIcing()
    {
        currentCake.SetIcing(icingType);

        Debug.Log(
            "Applied icing: " + icingType +
            " to " + currentCake.name
        );

        isTouching = false;
        contactTime = 0f;
        currentCake = null;
    }
}