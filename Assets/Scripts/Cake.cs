using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cake : MonoBehaviour
{
    [Header("Recipe")]
    public float milkAmount;
    public float flourAmount;
    public float eggAmount;
    public float sugarAmount;
    public float butterAmount;

    [Header("Flavour")]
    public CakeFlavour flavour;

    [Header("Icing")]

    public GameObject lemonIcing;
    public GameObject chocolateIcing;
    public GameObject strawberryIcing;

    [Header("Icing Application")]
    public float icingProgress = 0f;

    private IcingType currentIcingType;
    private bool isReceivingIcing = false;

    public float applyTime = 2f;

    [Header("Baking")]
    public float bakeProgress = 0f;

    public float bakedMin = 40f;
    public float bakedMax = 45f;

    public float bakeTick = 5f;

    [Header("Visual and Sound")]
    public Renderer cakeRenderer;
    public OvenAlarm ovenAlarm;
    public OneShotPlayer cakeFinished;

    private bool finishedSoundPlayed = false;
    private bool burntSoundPlayed = false;

    public Color batterColor = new Color(1f, 0.9f, 0.6f);
    public Color bakedColor = new Color32(196, 109, 33, 255);
    public Color burntColor = new Color32(60, 40, 20, 255);

    [Header("Flavour Color")]
    public Color flavourColor = new Color(1f, 0.9f, 0.6f);

    public XRSocketInteractor nextLayerSocket;
    public List<XRSocketInteractor> decorationSockets;

    public enum IcingType
    {
        None,
        Lemon,
        Chocolate,
        Strawberry
    }

    public IcingType icing = IcingType.None;

    public enum CakeFlavour
    {
        Plain,
        Vanilla,
        Strawberry,
        Chocolate
    }

    public void StartIcing(IcingType icingType)
    {
        currentIcingType = icingType;
        isReceivingIcing = true;
    }

    public void StopIcing()
    {
        isReceivingIcing = false;
    }

    private void Update()
    {
        UpdateIcing();
    }

    private void UpdateIcing()
    {
        if (!isReceivingIcing)
            return;

        if (icing == currentIcingType)
            return;

        icingProgress += Time.deltaTime;

        if (icingProgress >= applyTime)
        {
            SetIcing(currentIcingType);

            icingProgress = 0f;
            isReceivingIcing = false;

            Debug.Log(
                "Applied icing: " +
                currentIcingType
            );
        }
    }

    public void SetIcing(IcingType type)
    {
        icing = type;

        if (lemonIcing) lemonIcing.SetActive(type == IcingType.Lemon);
        if (chocolateIcing) chocolateIcing.SetActive(type == IcingType.Chocolate);
        if (strawberryIcing) strawberryIcing.SetActive(type == IcingType.Strawberry);
    }
    private Color GetBakedColor()
    {
        switch (flavour)
        {
            case CakeFlavour.Vanilla:
                return new Color32(196, 109, 33, 255);

            case CakeFlavour.Strawberry:
                return new Color32(180, 90, 110, 255);

            case CakeFlavour.Chocolate:
                return new Color32(90, 55, 30, 255);

            default:
                return new Color32(196, 109, 33, 255);
        }
    }

    private void Awake()
    {
        UpdateColor();
    }
    public void RefreshVisuals()
    {
        UpdateColor();
    }
    public void Bake()
    {
        bakeProgress += bakeTick * Time.deltaTime;

        UpdateColor();

        if (IsBaked() && !finishedSoundPlayed)
        {
            finishedSoundPlayed = true;

            cakeFinished.PlayOneShot();
        }

        if (IsBurnt() && !burntSoundPlayed)
        {
            burntSoundPlayed = true;

            ovenAlarm.OvenOn();
        }
    }

    public void StopOvenAlarm()
    {
        ovenAlarm.OvenOff();
    }

    private void UpdateColor()
    {
        if (cakeRenderer == null)
            return;

        if (bakeProgress <= bakedMax)
        {
            float t =
                Mathf.InverseLerp(
                    0f,
                    bakedMax,
                    bakeProgress
                );

            Color bakedFlavourColor = GetBakedColor();

            cakeRenderer.material.color =
                Color.Lerp(
                    batterColor,
                    bakedFlavourColor,
                    t
                );
        }
        else
        {
            float t =
                Mathf.InverseLerp(
                    bakedMax,
                    bakedMax + 20f,
                    bakeProgress
                );

            Color bakedFlavourColor = GetBakedColor();

            cakeRenderer.material.color =
                Color.Lerp(
                    bakedFlavourColor,
                    burntColor,
                    t
                );
        }
    }

    public bool IsRaw()
    {
        return bakeProgress < bakedMin;
    }

    public bool IsBaked()
    {
        return bakeProgress >= bakedMin &&
               bakeProgress <= bakedMax;
    }

    public bool IsBurnt()
    {
        return bakeProgress > bakedMax;
    }
    public CakeState GetState()
    {
        if (IsBurnt()) return CakeState.Burnt;
        if (IsBaked()) return CakeState.Baked;
        return CakeState.Raw;
    }
    public enum CakeState
    {
        Raw,
        Baked,
        Burnt
    }
}