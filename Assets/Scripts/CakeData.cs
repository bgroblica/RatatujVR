using UnityEngine;

public class CakeData : MonoBehaviour
{
    [Header("Recipe")]
    public float milkAmount;
    public float flourAmount;
    public float eggAmount;
    public float sugarAmount;
    public float butterAmount;

    [Header("Baking")]
    public float finalBakeProgress;
    public bool baked;
}