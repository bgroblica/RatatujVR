using System;
using UnityEngine;

public class Mold : MonoBehaviour
{
    private Vector3 initialPositionBatter;

    [Header("Material / Color")]
    public Renderer batterRenderer;
    public Color batterColor = new Color(1f, 0.9f, 0.6f);
    public Color cakeColor = new Color32(196, 109, 33, 255);

    [Header("Ingredients")]
    public float batterAmount = 0f;
    public float maxBatter = 8f;

    [Header("Baking")]
    public float bakeProgress = 0f;
    public float bakeTick = 0.1f;
    public float maxBake = 20f;
    public bool beinBaked = false;

    [Header("Visual")]
    public Transform batterVisual;
    public float maxBatterHeight = 0f;

    private void Awake()
    {
        Color currentColor = batterColor;
        initialPositionBatter = batterVisual.localPosition;
        if (batterRenderer != null)
        {
            batterRenderer.material.color = batterColor;
        }
    }
    public void AddBatter(float amount)
    {
        float total = batterAmount;

        float spaceLeft = maxBatter - total;

        float amountToAdd = Mathf.Min(amount, spaceLeft);

        batterAmount += amountToAdd;

        UpdateVisualBatter();
    }

    private void UpdateVisualBatter()
    {
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
    }

    public void Baking()
    {
        if (bakeProgress >= maxBake) return;

        bakeProgress += bakeTick * Time.deltaTime;
        bakeProgress = Mathf.Clamp(bakeProgress, 0, maxBake);

        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        float bakePercent = maxBake > 0 ? bakeProgress / maxBake : 0f;
        bakePercent = Mathf.Clamp01(bakePercent);

        Color currentColor = Color.Lerp(batterColor, cakeColor, bakePercent);

        batterRenderer.material.color = currentColor;
    }
    public bool HasBatter()
    {
        return batterAmount >= maxBatter;
    }
}
