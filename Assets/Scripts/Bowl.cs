using UnityEngine;

public class Bowl : MonoBehaviour
{
    private Vector3 initialScaleMilk;
    private Vector3 initialPositionMilk;
    private Vector3 initialScaleFlour;
    private Vector3 initialPositionFlour;

    public BatterBowl batterBowl;

    [Header("Material / Color")]
    public Renderer fillingRenderer;
    public Color milkColor = Color.white;
    public Color batterColor = new Color(1f, 0.9f, 0.6f);

    [Header("Ingredients")]
    public float milkAmount = 0f;
    public float flourAmount = 0f;
    public float maxIngredients = 5f;

    [Header("Mixing")]
    public float mixProgress = 0f;
    public float maxMix = 5f;

    [Header("Visual")]
    public Transform milkVisual;
    public float maxMilkHeight = 0.2f;
    public float milkSizeModifier = 1.32f;

    public Transform flourVisual;
    public float maxFlourHeight = 0.2f;
    public float flourSizeModifier = 1.32f;

    private void Awake()
    {
        initialScaleMilk = milkVisual.localScale;
        initialPositionMilk = milkVisual.localPosition;
        initialScaleFlour = flourVisual.localScale;
        initialPositionFlour = flourVisual.localPosition;
    }
    public void AddMilk(float amount)
    {
        float total = milkAmount + flourAmount;

        if (total >= maxIngredients) return;

        float spaceLeft = maxIngredients - total;

        float amountToAdd = Mathf.Min(amount, spaceLeft);

        milkAmount += amountToAdd;

        UpdateVisualMilk();
        batterBowl.UpdateAmount();
    }

    public void AddFlour(float amount)
    {
        float total = milkAmount + flourAmount;

        if (total >= maxIngredients) return;

        float spaceLeft = maxIngredients - total;

        float amountToAdd = Mathf.Min(amount, spaceLeft);

        flourAmount += amountToAdd;

        UpdateVisualFlour();
        batterBowl.UpdateAmount();
    }

    public void UpdateVisualMilk()
    {
        float normalized = maxIngredients > 0
            ? milkAmount / maxIngredients
            : 0f;

        normalized = Mathf.Clamp01(normalized);

        float newHeight = normalized * maxMilkHeight;

        milkVisual.localPosition = new Vector3(
            initialPositionMilk.x,
            initialPositionMilk.y,
            initialPositionMilk.z + newHeight
        );

        float scaleMultiplier = Mathf.Lerp(1f, milkSizeModifier, normalized);

        Vector3 scale = initialScaleMilk;
        scale.x *= scaleMultiplier;
        scale.y *= scaleMultiplier;

        milkVisual.localScale = scale;
    }

    private void UpdateVisualFlour()
    {
        float flourNormalized = maxIngredients > 0
            ? flourAmount / maxIngredients
            : 0f;

        flourNormalized = Mathf.Clamp01(flourNormalized);

        float mixPercent = maxMix > 0
            ? mixProgress / maxMix
            : 0f;

        mixPercent = Mathf.Clamp01(mixPercent);

        float remaining = Mathf.Lerp(flourNormalized, 0f, mixPercent);

        float newHeight = remaining * maxFlourHeight;

        flourVisual.localPosition = new Vector3(
            initialPositionFlour.x,
            initialPositionFlour.y,
            initialPositionFlour.z + (newHeight / 2f)
        );

        float scaleXY = Mathf.Lerp(1f, flourSizeModifier, remaining);

        Vector3 scale = initialScaleFlour;
        scale.x *= scaleXY;
        scale.y *= scaleXY;
        scale.z *= newHeight * 250f; 

        flourVisual.localScale = scale;
    }

    public void Mix(float intensity)
    {
        mixProgress += intensity * Time.deltaTime;
        mixProgress = Mathf.Clamp(mixProgress, 0, maxMix);

        UpdateMaterial();
        UpdateVisualFlour();
    }

    private void UpdateMaterial()
    {
        float mixPercent = maxMix > 0 ? mixProgress / maxMix : 0f;
        mixPercent = Mathf.Clamp01(mixPercent);

        Color currentColor = Color.Lerp(milkColor, batterColor, mixPercent);

        fillingRenderer.material.color = currentColor;
    }
    public bool IsFullyMixed()
    {
        return maxMix > 0 && (mixProgress / maxMix) >= 1f;
    }
    public float GetBatterAmount()
    {
        return milkAmount + flourAmount;
    }
}