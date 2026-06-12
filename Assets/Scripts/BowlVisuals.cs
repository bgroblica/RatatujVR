using UnityEngine;

public class BowlVisuals : MonoBehaviour
{
    public GameObject eggMesh;
    public GameObject milkMesh;
    public GameObject flourMesh;
    public GameObject sugarMesh;
    public GameObject butterMesh;

    public GameObject mixedMesh;

    public bool isMixed = false;


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


        if (isMixed == true)
        {
            eggMesh.SetActive(false);
            milkMesh.SetActive(false);
            flourMesh.SetActive(false);
            sugarMesh.SetActive(false);
            butterMesh.SetActive(false);

            mixedMesh.SetActive(true);
            Debug.Log("Mixed Mesh Active");
        }
        else
        {
            mixedMesh.SetActive(false);
        }
    }

    public void UpdateBatter()
    {
        if (isMixed == true)
        {
            eggMesh.SetActive(false);
            milkMesh.SetActive(false);
            flourMesh.SetActive(false);
            sugarMesh.SetActive(false);
            butterMesh.SetActive(false);

            mixedMesh.SetActive(true);
            Debug.Log("Mixed Mesh Active");
        }
        else
        {
            mixedMesh.SetActive(false);
            Debug.Log("Mixed Mesh Not Active");
        }
    }
}