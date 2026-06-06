using UnityEngine;
using static Computer;

public class GameFlowManager : MonoBehaviour
{
    public GameState currentState;

    [Header("Tutorial")]
    public ClientOrders tutorialOrder;

    [Header("Managers")]
    public OrderManager orderManager;


    public enum GameState
    {
        Tutorial,
        ChoosingOrder,
        ActiveOrder
    }
    private void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        currentState = GameState.Tutorial;

        orderManager.SetOrder(tutorialOrder);

        Debug.Log("Tutorial started");
    }

    public void CompleteTutorial()
    {
        currentState = GameState.ChoosingOrder;

        Debug.Log("Tutorial completed");
    }

    public void StartOrder(ClientOrders order)
    {
        orderManager.SetOrder(order);

        currentState = GameState.ActiveOrder;

        Debug.Log(
            "Started order: " +
            order.cakeName
        );
    }

    public void CompleteOrder()
    {
        currentState = GameState.ChoosingOrder;

        Debug.Log("Order completed");
    }
}