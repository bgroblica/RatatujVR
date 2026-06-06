using UnityEngine;

public class PrintedOrder : MonoBehaviour
{
    public void SetOrder(ClientOrders order)
    {
        Debug.Log(
            "Printed order: " + order.cakeName
        );
    }
}