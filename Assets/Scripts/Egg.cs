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

    [Header("Cracking")]
    public GameObject eggShellPrefab;
    public float crackForce = 2f;
    public float crackVelocity = 1f;

    private Rigidbody rb;
    private EggState eggState = EggState.Whole;

    private EggPourable eggPourable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        eggPourable = GetComponent<EggPourable>();
        if (eggPourable != null)
            eggPourable.enabled = false;

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
        if (eggState == newState) return;

        eggState = newState;

        if (eggState == EggState.Cracked)
        {
            SpawnShells();

            if (eggPourable != null)
            {
                eggPourable.enabled = true;
                eggPourable.Fill();
            }
        }

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

        foreach (var body in bodies)
        {
            body.AddForce(transform.forward * crackForce, ForceMode.Impulse);
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
                break;
        }
    }
}