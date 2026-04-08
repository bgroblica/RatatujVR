using UnityEngine;

public class Egg : Solids
{
    public float crackVelocity = 1.5f;
    public float eggAmount = 1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.magnitude < crackVelocity) return;

        Bowl bowl = collision.collider.GetComponentInParent<Bowl>();

        if (bowl != null)
        {
            bowl.AddEgg(eggAmount);
            Destroy(gameObject);
        }
    }
}