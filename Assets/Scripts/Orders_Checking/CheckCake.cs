using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CheckCake : MonoBehaviour
{
    public OrderManager orderManager;
    public XRSocketInteractor plateSocket;

    public int maxStars = 5;

    public int EvaluateCake()
    {
        var order = orderManager.GetActiveOrder();

        if (plateSocket == null)
        {
            Debug.LogError("No plate socket assigned!");
            return 0;
        }

        var interactable = plateSocket.GetOldestInteractableSelected();
        if (interactable == null)
        {
            Debug.Log("No plate in socket");
            return 0;
        }

        CakeStackReader reader =
            interactable.transform.GetComponent<CakeStackReader>();

        if (reader == null)
        {
            Debug.LogError("Socketed object has no CakeStackReader!");
            return 0;
        }

        var cake = reader.GetFullCake();

        float total = 0f;

        for (int i = 0; i < order.layers.Count; i++)
        {
            if (i >= cake.Count)
                break;

            total += EvaluateLayerScore(cake[i], order.layers[i]);
        }

        float average = total / order.layers.Count;

        int stars = Mathf.RoundToInt(average * 5f);

        Debug.Log("Cake score: " + stars + " stars");

        return stars;
    }
    private float EvaluateLayerScore(
        CakeStackReader.CakeLayerDataRuntime actual,
        CakeLayerData required)
    {
        float score = 0f;

        // ------------------------
        // 1. Bake state
        // ------------------------
        if (actual.cake.GetState() == required.requiredBakeState)
            score += 1f;

        // ------------------------
        // 2. Ingredients
        // ------------------------
        float ingredientScore =
            IngredientScore(actual.cake.milkAmount, required.milk) +
            IngredientScore(actual.cake.flourAmount, required.flour) +
            IngredientScore(actual.cake.eggAmount, required.egg) +
            IngredientScore(actual.cake.sugarAmount, required.sugar) +
            IngredientScore(actual.cake.butterAmount, required.butter);

        ingredientScore /= 5f;

        score += ingredientScore;

        // ------------------------
        // 3. Flavour
        // ------------------------
        float flavourScore =
            actual.cake.flavour == required.flavour
            ? 1f
            : 0f;

        score += flavourScore;

        // ------------------------
        // 4. Decorations
        // ------------------------
        int strawberries = 0;
        int lemons = 0;

        foreach (var deco in actual.decorations)
        {
            if (deco.CompareTag("Strawberry"))
                strawberries++;

            if (deco.CompareTag("Lemon"))
                lemons++;
        }

        float decoScore =
            (strawberries == required.strawberries ? 1f : 0f) +
            (lemons == required.lemons ? 1f : 0f);

        decoScore /= 2f;

        score += decoScore;

        // ------------------------
        // Final score (0-1)
        // ------------------------
        return score / 4f;
    }
    private float IngredientScore(float actual, float required)
{
    if (required <= 0f)
        return actual <= 0f ? 1f : 0f;

    float diff = Mathf.Abs(actual - required);
    float percentError = diff / required;

    return Mathf.Clamp01(1f - percentError);
}
}