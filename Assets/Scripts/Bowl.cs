using UnityEngine;

public class Bowl : MonoBehaviour
{
    private Vector3 initialScaleMilk;
    private Vector3 initialPositionMilk;
    private Vector3 initialScaleFlour;
    private Vector3 initialPositionFlour;

    public BatterBowl batterBowl;

    [Header("Ingredients")]
    public float milkAmount = 0f;
    public float flourAmount = 0f;
    public float eggAmount = 0f;
    public float sugarAmount = 0f;
    public float butterAmount = 0f;

    public float allIngredients = 0f;

    public float maxIngredients = 20f;

    [Header("Mixing")]
    public float mixProgress = 0f;
    public float maxMix = 5f;
    public float mixModifier = 1f;

    [Header("Mixed Batter Data")]
    public float batterAmount = 0f;

    public float batterMilk;
    public float batterFlour;
    public float batterEgg;
    public float batterSugar;
    public float batterButter;

    private bool batterCreated = false;

    [Header("Visual")]
    public Transform milkVisual;
    public float maxMilkHeight = 0.2f;
    public float milkSizeModifier = 1.32f;

    public Transform flourVisual;
    public float maxFlourHeight = 0.2f;
    public float flourSizeModifier = 1.32f;

    [Header("Material / Color")]
    public Renderer fillingRenderer;
    public Color milkColor = Color.white;
    public Color batterColor = new Color(1f, 0.9f, 0.6f);

    private void Awake()
    {
        initialScaleMilk = milkVisual.localScale;
        initialPositionMilk = milkVisual.localPosition;

        initialScaleFlour = flourVisual.localScale;
        initialPositionFlour = flourVisual.localPosition;
    }

    // ---------------------------
    // CLEANING (read-only logic)
    // ---------------------------
    private float Clean(float value)
    {
        if (Mathf.Abs(value) < 0.01f)
            return 0f;

        return value;
    }

    public float GetTotalIngredients()
    {
        return Clean(allIngredients);
    }

    // ---------------------------
    // INGREDIENT ADDING
    // ---------------------------
    public void AddMilk(float amount)
    {
        if (GetTotalIngredients() >= maxIngredients) return;

        milkAmount += amount;
        milkAmount = RoundIngredient(milkAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);
        UpdateVisualMilk();

        batterBowl.UpdateAmount();
    }

    public void AddFlour(float amount)
    {
        if (GetTotalIngredients() >= maxIngredients) return;

        flourAmount += amount;
        flourAmount = RoundIngredient(flourAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);
        UpdateVisualFlour();

        batterBowl.UpdateAmount();
    }

    public void AddEgg(float amount)
    {
        if (GetTotalIngredients() >= maxIngredients) return;

        eggAmount += amount;
        eggAmount = RoundIngredient(eggAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
    }

    public void AddSugar(float amount)
    {
        if (GetTotalIngredients() >= maxIngredients) return;

        sugarAmount += amount;
        sugarAmount = RoundIngredient(sugarAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
    }

    public void AddButter(float amount)
    {
        if (GetTotalIngredients() >= maxIngredients) return;

        butterAmount += amount;
        butterAmount = RoundIngredient(butterAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
    }

    // ---------------------------
    // VISUALS
    // ---------------------------
    public void UpdateVisualMilk()
    {
        float normalized = milkAmount / maxIngredients;
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
        float flourNormalized = flourAmount / maxIngredients;
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

    // ---------------------------
    // MIXING
    // ---------------------------
    public void Mix(float intensity)
    {
        mixProgress += intensity * Time.deltaTime * mixModifier;
        mixProgress = Mathf.Clamp(mixProgress, 0, maxMix);

        UpdateMaterial();
        UpdateVisualFlour();
        if (IsFullyMixed() && !batterCreated)
        {
            CreateBatter();
        }
    }
    private float RoundIngredient(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }
    private void CreateBatter()
    {
        batterCreated = true;

        // SAVE RECIPE DATA
        batterMilk = milkAmount;
        batterFlour = flourAmount;
        batterEgg = eggAmount;
        batterSugar = sugarAmount;
        batterButter = butterAmount;

        // CREATE FINAL BATTER
        batterAmount =
            milkAmount +
            flourAmount +
            eggAmount +
            sugarAmount +
            butterAmount;

        // CLEAR RAW INGREDIENTS
        milkAmount = 0f;
        flourAmount = 0f;
        eggAmount = 0f;
        sugarAmount = 0f;
        butterAmount = 0f;

        allIngredients = 0f;

        batterBowl.SetFilled();

        Debug.Log("Batter created");
    }

    private void UpdateMaterial()
    {
        float mixPercent = maxMix > 0 ? mixProgress / maxMix : 0f;
        mixPercent = Mathf.Clamp01(mixPercent);

        Color currentColor = Color.Lerp(milkColor, batterColor, mixPercent);

        fillingRenderer.material.color = currentColor;
    }

    // ---------------------------
    // STATE CHECKS
    // ---------------------------
    public bool IsFullyMixed()
    {
        return maxMix > 0 && (mixProgress / maxMix) >= 1f;

    }

    public float GetBatterAmount()
    {
        return Clean(batterAmount);
    }

    public void ResetBowl()
    {
        milkAmount = 0f;
        flourAmount = 0f;
        eggAmount = 0f;
        sugarAmount = 0f;
        butterAmount = 0f;

        allIngredients = 0f;

        batterAmount = 0f;

        batterMilk = 0f;
        batterFlour = 0f;
        batterEgg = 0f;
        batterSugar = 0f;
        batterButter = 0f;

        batterCreated = false;

        mixProgress = 0f;

        UpdateVisualMilk();
        UpdateVisualFlour();
        UpdateMaterial();
    }
}