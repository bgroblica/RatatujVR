using UnityEngine;

public class Bowl : MonoBehaviour
{
    private Vector3 initialScale;
    private Vector3 initialPosition;

    [Header("Milk")]
    public float milkAmount = 0f;
    public float flourAmount = 0f;
    public float maxIngredients = 5f;

    [Header("Visual")]
    public Transform fillingVisual;
    public float maxHeight = 0.2f;

    private void Awake()
    {
        initialScale = fillingVisual.localScale;
        initialPosition = fillingVisual.localPosition;
    }
    public void AddMilk(float amount)
    {
        float total = milkAmount + flourAmount;

        if (total >= maxIngredients) return;

        float spaceLeft = maxIngredients - total;

        float amountToAdd = Mathf.Min(amount, spaceLeft);

        milkAmount += amountToAdd;

        UpdateVisual();
    }

    public void AddFlour(float amount)
    {
        float total = milkAmount + flourAmount;

        if (total >= maxIngredients) return;

        float spaceLeft = maxIngredients - total;

        float amountToAdd = Mathf.Min(amount, spaceLeft);

        flourAmount += amountToAdd;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        float total = milkAmount + flourAmount;

        float normalized = maxIngredients > 0
            ? total / maxIngredients
            : 0f;

        normalized = Mathf.Clamp01(normalized);

        float newHeight = normalized * maxHeight;

        fillingVisual.localPosition = new Vector3(
            initialPosition.x,
            initialPosition.y,
            initialPosition.z + newHeight
        );

        float scaleMultiplier = Mathf.Lerp(1f, 1.32f, normalized);

        Vector3 scale = initialScale;
        scale.x *= scaleMultiplier;
        scale.y *= scaleMultiplier;

        fillingVisual.localScale = scale;
    }
}