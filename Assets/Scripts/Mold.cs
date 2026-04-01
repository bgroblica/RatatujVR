using System;
using UnityEngine;

public class Mold : MonoBehaviour
{
    private Vector3 initialScaleBatter;
    private Vector3 initialPositionBatter;

    private Renderer batterRenderer;
    private GameObject currentBatter;

    [Header("Batter Prefab")]
    public GameObject batterPrefab;
    public Transform batterVisual;

    [Header("Material / Color")]
    public Color batterColor = new Color(1f, 0.9f, 0.6f);
    public Color cakeColor = new Color32(196, 109, 33, 255);

    [Header("Ingredients")]
    public float batterAmount = 0f;
    public float maxBatter = 8f;

    [Header("Baking")]
    public float bakeProgress = 0f;
    public float bakeTick = 0.1f;
    public float maxBake = 20f;

    [Header("Visual")]
    public float maxBatterHeight = 0.2f;
    public float batterSizeModifier = 1.32f;

    private void Awake()
    {
        batterAmount = 0f;
        bakeProgress = 0f;
        initialPositionBatter = batterVisual.localPosition;
        initialScaleBatter = batterVisual.localScale;
    }

    public void AddBatter(float amount)
    {
        if (currentBatter == null)
        {
            currentBatter = Instantiate(
                batterPrefab,
                batterVisual
            );

            batterRenderer = currentBatter.GetComponentInChildren<Renderer>();

            if (batterRenderer != null)
            {
                batterRenderer.material.color = batterColor;
            }
        }

        float total = batterAmount;
        float spaceLeft = maxBatter - total;
        float amountToAdd = Mathf.Min(amount, spaceLeft);

        batterAmount += amountToAdd;

        UpdateVisualBatter();
    }

    private void UpdateVisualBatter()
    {
        if (batterVisual == null) return;

        float normalized = maxBatter > 0
            ? batterAmount / maxBatter
            : 0f;

        normalized = Mathf.Clamp01(normalized);

        float newHeight = normalized * maxBatterHeight;

        batterVisual.localPosition = new Vector3(
            initialPositionBatter.x,
            initialPositionBatter.y + newHeight,
            initialPositionBatter.z
        );

        float scaleY = Mathf.Lerp(1f, batterSizeModifier, normalized);

        Vector3 scale = initialScaleBatter;
        scale.y *= scaleY;

        batterVisual.localScale = scale;
    }

    public void Baking()
    {
        if (!HasBatter()) return;
        if (bakeProgress >= maxBake) return;

        bakeProgress += bakeTick * Time.deltaTime;
        bakeProgress = Mathf.Clamp(bakeProgress, 0, maxBake);

        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (batterRenderer == null) return;

        float bakePercent = maxBake > 0
            ? bakeProgress / maxBake
            : 0f;

        bakePercent = Mathf.Clamp01(bakePercent);

        Color currentColor = Color.Lerp(
            batterColor,
            cakeColor,
            bakePercent
        );

        batterRenderer.material.color = currentColor;
    }

    public bool HasBatter()
    {
        return batterAmount >= maxBatter;
    }

    public bool IsFullyBaked()
    {
        return bakeProgress >= maxBake;
    }

    public void ReleaseCake()
    {
        Transform holder = currentBatter.transform.Find("ColliderHolder");
        if (holder != null)
        {
            holder.gameObject.SetActive(true);
        }

        currentBatter.transform.SetParent(null);

        Rigidbody rb = currentBatter.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        currentBatter = null;
        batterVisual = null;
        batterRenderer = null;

        batterAmount = 0f;
        bakeProgress = 0f;
    }
}