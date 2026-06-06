using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static GameFlowManager;

public class CheckCake : MonoBehaviour
{
    public OrderManager orderManager;
    public XRSocketInteractor plateSocket;

    public GameFlowManager gameFlowManager;

    public int maxStars = 5;

    public void CheckCurrentCake()
    {
        int stars = EvaluateCake();

        Debug.Log("Final Stars: " + stars);

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

        if (bakeCorrect)
            score += 1f;

        // ------------------------
        // 2. Ingredients
        // ------------------------
        float milkScore =
            IngredientScore(
                actual.cake.milkAmount,
                required.milk
            );

        float flourScore =
            IngredientScore(
                actual.cake.flourAmount,
                required.flour
            );

        float eggScore =
            IngredientScore(
                actual.cake.eggAmount,
                required.egg
            );

        float sugarScore =
            IngredientScore(
                actual.cake.sugarAmount,
                required.sugar
            );

        float butterScore =
            IngredientScore(
                actual.cake.butterAmount,
                required.butter
            );

        Debug.Log(
            $"Milk: {actual.cake.milkAmount} / {required.milk} = {milkScore:F2}"
        );

        Debug.Log(
            $"Flour: {actual.cake.flourAmount} / {required.flour} = {flourScore:F2}"
        );

        Debug.Log(
            $"Egg: {actual.cake.eggAmount} / {required.egg} = {eggScore:F2}"
        );

        Debug.Log(
            $"Sugar: {actual.cake.sugarAmount} / {required.sugar} = {sugarScore:F2}"
        );

        Debug.Log(
            $"Butter: {actual.cake.butterAmount} / {required.butter} = {butterScore:F2}"
        );

        float ingredientScore =
            milkScore +
            flourScore +
            eggScore +
            sugarScore +
            butterScore;

        ingredientScore /= 5f;

        Debug.Log(
            $"Ingredient Score: {ingredientScore:F2}"
        );

        score += ingredientScore;

        // ------------------------
        // 3. Flavour
        // ------------------------
        float flavourScore =
            actual.cake.flavour == required.flavour
            ? 1f
            : 0f;

        Debug.Log(
            $"Flavour: {actual.cake.flavour} | " +
            $"Required: {required.flavour} | " +
            $"Score: {flavourScore:F2}"
        );

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

        Debug.Log(
            $"Strawberries: {strawberries}/{required.strawberries}"
        );

        Debug.Log(
            $"Lemons: {lemons}/{required.lemons}"
        );

        float decoScore =
            (strawberries == required.strawberries ? 1f : 0f) +
            (lemons == required.lemons ? 1f : 0f);

        decoScore /= 2f;

        Debug.Log(
            $"Decoration Score: {decoScore:F2}"
        );

        score += decoScore;

        float finalLayerScore = score / 4f;

        Debug.Log(
            $"Layer Final Score: {finalLayerScore:F2}"
        );

        return finalLayerScore;
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