using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientOrders", menuName = "Scriptable Objects/ClientOrders")]
public class ClientOrders : ScriptableObject
{
    [Header("Name")]
    public string cakeName;

    public string maxIngredients = "Big is 27, medium is 18, small is 9";

    [Header("Layers (1–3 cakes stacked)")]
    public List<CakeLayerData> layers = new List<CakeLayerData>();

    [Header("Frosting (later)")]
    public string frostingType;
    public string frostingSomething;
}