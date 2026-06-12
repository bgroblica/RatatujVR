using UnityEngine;

public class Bowl : MonoBehaviour
{
    public BatterBowl batterBowl;


    [Header("Ingredients")]
    public float milkAmount = 0f;
    public float flourAmount = 0f;
    public float eggAmount = 0f;
    public float sugarAmount = 0f;
    public float butterAmount = 0f;

    public float allIngredients = 0f;

    public float maxIngredients = 20f;

    [Header("Flavour")]
    public Cake.CakeFlavour flavour = Cake.CakeFlavour.Plain;

    public Cake.CakeFlavour batterFlavour;

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
    public BowlVisuals visuals;


    [Header("Material / Color")]
    public Renderer fillingRenderer;
    public Color batterColor = new Color(1f, 0.9f, 0.6f);

    [Header("Flavour Color")]
    public Color flavourColor = new Color(1f, 0.9f, 0.6f);

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
    public void SetFlavour(Cake.CakeFlavour newFlavour)
    {
        flavour = newFlavour;

        Debug.Log("Flavour set to: " + flavour);
    }
    public void SetFlavourColor(Color color)
    {
        flavourColor = color;
        Debug.Log("Flavour color set to: " + flavourColor);
    }
    public void AddMilk(float amount)
    {
        float spaceLeft = maxIngredients - allIngredients;

        if (spaceLeft <= 0f)
            return;

        amount = Mathf.Min(amount, spaceLeft);

        milkAmount += amount;
        milkAmount = RoundIngredient(milkAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

     //   UpdateVisualMilk();
        batterBowl.UpdateAmount();
        RefreshVisuals();
    }

    public void AddFlour(float amount)
    {
        float spaceLeft = maxIngredients - allIngredients;

        if (spaceLeft <= 0f)
            return;

        amount = Mathf.Min(amount, spaceLeft);

        flourAmount += amount;
        flourAmount = RoundIngredient(flourAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

     //   UpdateVisualFlour();
        batterBowl.UpdateAmount();
        RefreshVisuals();
    }

    public void AddEgg(float amount)
    {
        float spaceLeft = maxIngredients - allIngredients;

        if (spaceLeft <= 0f)
            return;

        amount = Mathf.Min(amount, spaceLeft);

        eggAmount += amount;
        eggAmount = RoundIngredient(eggAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
        RefreshVisuals();
    }

    public void AddSugar(float amount)
    {
        float spaceLeft = maxIngredients - allIngredients;

        if (spaceLeft <= 0f)
            return;

        amount = Mathf.Min(amount, spaceLeft);

        sugarAmount += amount;
        sugarAmount = RoundIngredient(sugarAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
        RefreshVisuals();
    }

    public void AddButter(float amount)
    {
        float spaceLeft = maxIngredients - allIngredients;

        if (spaceLeft <= 0f)
            return;

        amount = Mathf.Min(amount, spaceLeft);

        butterAmount += amount;
        butterAmount = RoundIngredient(butterAmount);

        allIngredients += amount;
        allIngredients = RoundIngredient(allIngredients);

        batterBowl.UpdateAmount();
        RefreshVisuals();
    }

    // ---------------------------
    // VISUALS
    // ---------------------------

    void RefreshVisuals()
    {
        visuals.UpdateVisuals(
            eggAmount,
            milkAmount,
            flourAmount,
            sugarAmount,
            butterAmount
        );
    }

    // ---------------------------
    // MIXING
    // ---------------------------
    public void Mix(float intensity)
    {
        mixProgress += intensity * Time.deltaTime * mixModifier;
        mixProgress = Mathf.Clamp(mixProgress, 0, maxMix);

        UpdateMaterial();
      //  UpdateVisualFlour();
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
        visuals.isMixed = true;
        batterCreated = true;
        visuals.UpdateBatter();


        // SAVE RECIPE DATA
        batterMilk = milkAmount;
        batterFlour = flourAmount;
        batterEgg = eggAmount;
        batterSugar = sugarAmount;
        batterButter = butterAmount;

        batterFlavour = flavour;

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
        batterBowl.UpdateAmount();

        Debug.Log("Batter created");
    }

    private void UpdateMaterial()
    {
        float mixPercent = maxMix > 0 ? mixProgress / maxMix : 0f;
        mixPercent = Mathf.Clamp01(mixPercent);

        Color currentColor = Color.Lerp(flavourColor, batterColor, mixPercent);

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

        flavour = Cake.CakeFlavour.Plain;

        flavourColor =
            new Color(1f, 0.9f, 0.6f);
        batterFlavour = Cake.CakeFlavour.Plain;

        batterCreated = false;

        mixProgress = 0f;

        visuals.isMixed = false;

        UpdateMaterial();
        visuals.UpdateBatter();
    }
}