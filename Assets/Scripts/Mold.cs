using UnityEngine;

public class Mold : MonoBehaviour
{
    private Vector3 initialScaleBatter;
    private Vector3 initialPositionBatter;

    private GameObject currentBatter;

    [Header("Batter Prefab")]
    public GameObject batterPrefab;
    public Transform batterVisual;

    [Header("Recipe Data")]
    public float milkAmount;
    public float flourAmount;
    public float eggAmount;
    public float sugarAmount;
    public float butterAmount;

    [Header("Ingredients")]
    public float batterAmount = 0f;
    public float maxBatter = 8f;

    [Header("Flavour")]
    public Cake.CakeFlavour flavour;

    [Header("Visual")]
    public float maxBatterHeight = 0.2f;
    public float batterSizeModifier = 1.32f;

    [Header("Flavour Color")]
    public Color flavourColor = new Color(1f, 0.9f, 0.6f);

    private void Awake()
    {
        batterAmount = 0f;

        initialPositionBatter =
            batterVisual.localPosition;

        initialScaleBatter =
            batterVisual.localScale;
    }

    public void AddBatter(
        float amount,
        float milk,
        float flour,
        float egg,
        float sugar,
        float butter,
        Cake.CakeFlavour flavour,
        Color flavourColor
    )
    {
        if (currentBatter == null)
        {
            currentBatter =
                Instantiate(
                    batterPrefab,
                    batterVisual
                );
        }

        if (batterAmount <= 0f)
        {
            milkAmount = milk;
            flourAmount = flour;
            eggAmount = egg;
            sugarAmount = sugar;
            butterAmount = butter;

            this.flavour = flavour;
            this.flavourColor = flavourColor;
        }

        float spaceLeft =
            maxBatter - batterAmount;

        float amountToAdd =
            Mathf.Min(amount, spaceLeft);

        batterAmount += amountToAdd;

        UpdateVisualBatter();
    }

    private void UpdateVisualBatter()
    {
        if (batterVisual == null)
            return;

        float normalized =
            maxBatter > 0
            ? batterAmount / maxBatter
            : 0f;

        normalized =
            Mathf.Clamp01(normalized);

        float newHeight =
            normalized * maxBatterHeight;

        batterVisual.localPosition =
            new Vector3(
                initialPositionBatter.x,
                initialPositionBatter.y + newHeight,
                initialPositionBatter.z
            );

        float scaleY =
            Mathf.Lerp(
                1f,
                batterSizeModifier,
                normalized
            );

        Vector3 scale =
            initialScaleBatter;

        scale.y *= scaleY;

        batterVisual.localScale =
            scale;
    }

    public bool HasBatter()
    {
        return batterAmount >= maxBatter;
    }

    public GameObject GetCake()
    {
        return currentBatter;
    }

    public void ReleaseCake()
    {
        if (currentBatter == null)
            return;

        GameObject cakeObject =
            currentBatter;

        currentBatter = null;

        Cake cake =
            cakeObject.GetComponent<Cake>();

        if (cake != null)
        {
            cake.flavourColor = flavourColor;
            cake.batterColor = flavourColor;

            cake.milkAmount = milkAmount;
            cake.flourAmount = flourAmount;
            cake.eggAmount = eggAmount;
            cake.sugarAmount = sugarAmount;
            cake.butterAmount = butterAmount;
        }

        cakeObject.transform.SetParent(
            null,
            true
        );

        cakeObject.transform.position +=
            -transform.up * 0.05f;

        Transform holder =
            cakeObject.transform.Find(
                "ColliderHolder"
            );

        if (holder != null)
        {
            holder.gameObject.SetActive(true);
        }

        Rigidbody rb =
            cakeObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        batterAmount = 0f;

        milkAmount = 0f;
        flourAmount = 0f;
        eggAmount = 0f;
        sugarAmount = 0f;
        butterAmount = 0f;

        flavour = Cake.CakeFlavour.Deafult;
        flavourColor = new Color(1f, 0.9f, 0.6f);
    }
}