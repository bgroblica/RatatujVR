using UnityEngine;

[System.Serializable]
public class CakeLayerData
{
    [Header("Recipe")]
    public float milk;
    public float flour;
    public float egg;
    public float sugar;
    public float butter;

    [Header("Decorations")]
    public int strawberries;
    public int lemons;

    [Header("Baking")]
    public Cake.CakeState requiredBakeState;
}