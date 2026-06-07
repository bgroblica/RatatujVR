using UnityEngine;

public class BowlVisuals : MonoBehaviour
{
    public GameObject eggMesh;
    public GameObject milkMesh;
    public GameObject flourMesh;
    public GameObject sugarMesh;
    public GameObject butterMesh;

    public GameObject mixedMesh;

    public void UpdateVisuals(
        float egg,
        float milk,
        float flour,
        float sugar,
        float butter)
    {
        eggMesh.SetActive(egg > 0);
        milkMesh.SetActive(milk > 0);
        flourMesh.SetActive(flour > 0);
        sugarMesh.SetActive(sugar > 0);
        butterMesh.SetActive(butter > 0);

        bool isMixed =
            egg > 0 && milk > 0 && flour > 0 && sugar > 0 && butter > 0;

        if (isMixed)
        {
            eggMesh.SetActive(false);
            milkMesh.SetActive(false);
            flourMesh.SetActive(false);
            sugarMesh.SetActive(false);
            butterMesh.SetActive(false);

            mixedMesh.SetActive(true);
        }
        else
        {
            mixedMesh.SetActive(false);
        }
    }
}