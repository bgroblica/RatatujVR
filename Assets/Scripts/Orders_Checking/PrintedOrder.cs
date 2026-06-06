using UnityEngine;

public class PrintedOrder : MonoBehaviour
{
    public Renderer screenRenderer;

    public void SetOrder(ClientOrders order)
    {
        // later you can swap material / texture here
        Debug.Log("Printed order: " + order.cakeName);
    }
}