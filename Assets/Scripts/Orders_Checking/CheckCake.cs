using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static GameFlowManager;

public class CheckCake : MonoBehaviour
{
    public OrderManager orderManager;
    public XRSocketInteractor plateSocket;
    public GameFlowManager gameFlowManager;
    public Printer printer;

    public Animator boxAnimator;

    public OneShotPlayer cakeSend;
    public OneShotPlayer machineSending;

    [Header("Judging")]
    public float judgingDelay = 2f;

    public float destroyDelay = 0.5f;


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
        StartCoroutine(CheckCakeRoutine());

        cakeSend.PlayOneShot();

        if (boxAnimator != null)
        {
            boxAnimator.SetBool("Closed", true);
        }
    }
    private IEnumerator CheckCakeRoutine()
    {

        yield return new WaitForSeconds(judgingDelay);

        float score = EvaluateCake();

        Debug.Log("Final Score: " + score);

        if (printer != null)
        {
            printer.PrintResult(score);
        }

        yield return new WaitForSeconds(destroyDelay);

        DestroyCurrentCake();


        if (boxAnimator != null)
        {
            boxAnimator.SetBool("Closed", false);
            machineSending.PlayOneShot();
        }

        if (gameFlowManager == null)
            yield break;

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

    public float EvaluateCake()
    {
        var order = orderManager.GetActiveOrder();

        if (order == null)
        {
            Debug.LogError("No active order!");
            return 0f;
        }

        if (plateSocket == null)
        {
            Debug.LogError("No plate socket assigned!");
            return 0f;
        }

        var interactable = plateSocket.GetOldestInteractableSelected();
        if (interactable == null)
        {
            Debug.Log("No plate in socket");
            return 0f;
        }

        CakeStackReader reader =
            interactable.transform.GetComponent<CakeStackReader>();

        if (reader == null)
        {
            Debug.LogError("Socketed object has no CakeStackReader!");
            return 0f;
        }

        var cake = reader.GetFullCake();

        if (order.layers.Count == 0 || cake.Count == 0)
        {
            Debug.LogWarning("Empty cake or order");
            return 0f;
        }

        float total = 0f;
        int layerCount = Mathf.Min(order.layers.Count, cake.Count);

        for (int i = 0; i < layerCount; i++)
        {
            total += EvaluateLayerScore(cake[i], order.layers[i]);
        }

        float average = total / layerCount;

        Debug.Log("========== FINAL RESULT ==========");
        Debug.Log("Average Score: " + average.ToString("F2"));

        return Mathf.Clamp01(average);
    }

    private float EvaluateLayerScore(
        CakeStackReader.CakeLayerDataRuntime actual,
        CakeLayerData required)
    {
        float score = 0f;

        // ------------------------
        // 1. Bake
        // ------------------------
        float bakeScore =
            actual.cake.GetState() == required.requiredBakeState ? 1f : 0f;

        score += bakeScore;

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

        // ------------------------
        // 3. Flavour
        // ------------------------
        float flavourScore =
            actual.cake.flavour == required.flavour ? 1f : 0f;

        // ------------------------
        // 4. Icing
        // ------------------------
        float icingScore =
            actual.cake.icing == required.requiredIcing ? 1f : 0f;

        // ------------------------
        // 5. Decorations (continuous)
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

        Debug.Log($"Layer Score: {finalScore:F2}");

        return finalScore;
    }

    // ------------------------
    private float IngredientScore(float actual, float required)
    {
        if (required <= 0f)
            return actual <= 0f ? 1f : 0f;

        float diff = Mathf.Abs(actual - required);
        float percentError = diff / required;

        return Mathf.Clamp01(1f - percentError);
    }

    private float DecorationScore(int actual, int required)
    {
        if (required <= 0)
            return actual == 0 ? 1f : 0f;

        float diff = Mathf.Abs(actual - required);
        float percentError = diff / required;

        return Mathf.Clamp01(1f - percentError);
    }
    private void DestroyCurrentCake()
    {
        var interactable =
            plateSocket.GetOldestInteractableSelected();

        if (interactable == null)
            return;

        Destroy(interactable.transform.root.gameObject);
    }
}