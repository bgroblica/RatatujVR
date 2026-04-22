using UnityEngine;

public class Egg : Solids
{
    public enum EggState
    {
        Whole,
        Cracked
    }

    [Header("Mesh")]
    public MeshFilter meshFilter;
    public Mesh wholeEgg;
    public Mesh crackedEgg;

    public GameObject eggShellPrefab;

    public float crackForce = 2f;

    public float crackVelocity = 1.5f;

    private Rigidbody rb;
    private EggState eggState = EggState.Whole;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetState(EggState.Whole);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (eggState != EggState.Whole) return;

        if (rb.linearVelocity.magnitude < crackVelocity)
            return;

        SetState(EggState.Cracked);
        Debug.Log("Egg Cracked");
    }

    private void SetState(EggState newState)
    {
        eggState = newState;
        UpdateVisuals();
    }

    private void SpawnShells()
    {
        if (eggShellPrefab == null) return;

        GameObject shell = Instantiate(
            eggShellPrefab,
            transform.position,
            transform.rotation
        );

        Rigidbody[] bodies = shell.GetComponentsInChildren<Rigidbody>();

        foreach (var rb in bodies)
        {
            rb.AddForce(transform.forward * crackForce, ForceMode.Impulse);
        }
    }

    private void UpdateVisuals()
    {
        switch (eggState)
        {
            case EggState.Whole:
                meshFilter.mesh = wholeEgg;
                break;

            case EggState.Cracked:
                meshFilter.mesh = crackedEgg;
                SpawnShells();
                break;
        }
    }
}