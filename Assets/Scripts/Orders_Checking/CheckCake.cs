using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static GameFlowManager;

public class CheckCake : MonoBehaviour
{
    public OrderManager orderManager;
    public XRSocketInteractor plateSocket;
    public GameFlowManager gameFlowManager;
    public Printer printer;

    public int maxStars = 5;

    // ---------- WEIGHTS ----------
    private const float bakeWeight = 1f;
    private const float ingredientWeight = 1f;
    private const float flavourWeight = 1f;
    private const float icingWeight = 1f;
    private const float decorationWeight = 1f;

    private const float totalWeight =
        bakeWeight +
        ingredientWeight +
        flavourWeight +
        icingWeight +
        decorationWeight;

    // -----------------------------

    public void CheckCurrentCake()
    {
        int stars = EvaluateCake();

        Debug.Log("Final Stars: " + stars);

        if (printer != null)
        {
            printer.PrintResult(stars);
        }

        if (gameFlowManager == null)
            return;

        if (gameFlowManager.currentState ==
            GameState.Tutorial)
        {
            gameFlowManager.CompleteTutorial();
        }
        else if (gameFlowManager.currentState ==
                 GameState.ActiveOrder)
        {
            gameFlowManager.CompleteOrder();
        }
    }

    public int EvaluateCake()
    {
        var order = orderManager.GetActiveOrder();

        if (order == null)
        {
            Debug.LogError("No active order!");
            return 0;
        }

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

        if (order.layers.Count == 0 || cake.Count == 0)
        {
            Debug.LogWarning("Empty cake or order");
            return 0;
        }

        float total = 0f;

        int layerCount = Mathf.Min(order.layers.Count, cake.Count);

        for (int i = 0; i < layerCount; i++)
        {
            total += EvaluateLayerScore(
                cake[i],
                order.layers[i]
            );
        }

        float average = total / layerCount;

        int stars =
            Mathf.Clamp(
                Mathf.RoundToInt(average * 5f),
                0,
                maxStars
            );

        Debug.Log("========== FINAL RESULT ==========");
        Debug.Log("Average Score: " + average.ToString("F2"));
        Debug.Log("Stars: " + stars + "/" + maxStars);

        return stars;
    }

    private float EvaluateLayerScore(
        CakeStackReader.CakeLayerDataRuntime actual,
        CakeLayerData required)
    {
        float score = 0f;

        Debug.Log("========== CAKE LAYER ==========");

        // ------------------------
        // 1. Bake state
        // ------------------------
        bool bakeCorrect =
            actual.cake.GetState() ==
            required.requiredBakeState;

        Debug.Log(
            $"Bake State: {actual.cake.GetState()} | " +
            $"Required: {required.requiredBakeState} | " +
            $"Correct: {bakeCorrect}"
        );

        score += bakeCorrect ? 1f : 0f;

        float bakeScore = bakeCorrect ? 1f : 0f;

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

        Debug.Log($"Ingredient Score: {ingredientScore:F2}");

        // ------------------------
        // 3. Flavour
        // ------------------------
        float flavourScore =
            actual.cake.flavour == required.flavour ? 1f : 0f;

        Debug.Log(
            $"Flavour: {actual.cake.flavour} | " +
            $"Required: {required.flavour} | " +
            $"Score: {flavourScore:F2}"
        );

        // ------------------------
        // 4. Icing (SAFE)
        // ------------------------
        float icingScore =
            actual.cake.icing == required.requiredIcing ? 1f : 0f;

        Debug.Log(
            $"Icing: {actual.cake.icing} | " +
            $"Required: {required.requiredIcing} | " +
            $"Score: {icingScore:F2}"
        );

        // ------------------------
        // 5. Decorations (FIXED)
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

        float strawberryScore =
            DecorationScore(strawberries, required.strawberries);

        float lemonScore =
            DecorationScore(lemons, required.lemons);

        float decorationScore =
            (strawberryScore + lemonScore) / 2f;

        Debug.Log(
            $"Strawberries: {strawberries}/{required.strawberries} | Score: {strawberryScore:F2}"
        );

        Debug.Log(
            $"Lemons: {lemons}/{required.lemons} | Score: {lemonScore:F2}"
        );

        Debug.Log(
            $"Decoration Score: {decorationScore:F2}"
        );

        // ------------------------
        // FINAL WEIGHTED SCORE
        // ------------------------
        float finalScore =
            (bakeScore * bakeWeight) +
            (ingredientScore * ingredientWeight) +
            (flavourScore * flavourWeight) +
            (icingScore * icingWeight) +
            (decorationScore * decorationWeight);

        finalScore /= totalWeight;

        Debug.Log($"Layer Final Score: {finalScore:F2}");

        return finalScore;
    }

    // ------------------------
    // Ingredient helper
    // ------------------------
    private float IngredientScore(float actual, float required)
    {
        if (required <= 0f)
            return actual <= 0f ? 1f : 0f;

        float diff = Mathf.Abs(actual - required);
        float percentError = diff / required;

        return Mathf.Clamp01(1f - percentError);
    }

    // ------------------------
    // NEW: Decoration scoring
    // ------------------------
    private float DecorationScore(int actual, int required)
    {
        if (required <= 0)
            return actual == 0 ? 1f : 0f;

        float diff = Mathf.Abs(actual - required);
        float percentError = diff / required;

        return Mathf.Clamp01(1f - percentError);
    }
}