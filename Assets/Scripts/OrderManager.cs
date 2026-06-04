using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private ClientOrders activeOrder;

    public ClientOrders GetActiveOrder()
    {
        return activeOrder;
    }

    public void SetOrder(ClientOrders newOrder)
    {
        activeOrder = newOrder;
    }

    private void Start()
    {
        if (activeOrder == null)
        {
            Debug.LogWarning("OrderManager: No active order assigned!");
        }
    }
}