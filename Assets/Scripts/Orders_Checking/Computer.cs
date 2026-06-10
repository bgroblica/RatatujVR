using System.Collections.Generic;
using UnityEngine;
using static GameFlowManager;

public class Computer : MonoBehaviour
{
    [Header("Refs")]
    public GameFlowManager gameFlow;
    public Printer printer;

    [Header("Screen Material")]

    public Renderer screenRenderer;

    public Material tutorialMaterial;
    public Material tutorialFinishedMaterial;


    [Header("Orders")]
    public List<ClientOrders> availableOrders;

    private int currentIndex = 0;

    public ClientOrders CurrentOrder =>
        availableOrders[currentIndex];

    private void Start()
    {
        RefreshScreen();
    }

    public void Left()
    {
        if (gameFlow.currentState != GameState.ChoosingOrder)
            return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = availableOrders.Count - 1;

        Debug.Log(
            $"Current Index: {currentIndex} | " +
            $"Order: {CurrentOrder.cakeName}"
        );

        RefreshScreen();
    }

    public void Right()
    {
        if (gameFlow.currentState != GameState.ChoosingOrder)
            return;

        currentIndex++;

        if (currentIndex >= availableOrders.Count)
            currentIndex = 0;

        Debug.Log(
            $"Current Index: {currentIndex} | " +
            $"Order: {CurrentOrder.cakeName}"
        );

        RefreshScreen();
    }

    public void Print()
    {
        if (gameFlow.currentState == GameState.Tutorial)
        {
            Debug.Log("Print Tutorial");

            if (printer != null)
            {
                printer.PrintTutorialPage();
            }

            return;
        }

        if (gameFlow.currentState == GameState.ChoosingOrder)
        {
            Debug.Log("Accept Order: " + CurrentOrder.cakeName);

            gameFlow.StartOrder(CurrentOrder);

            if (printer != null)
            {
                printer.PrintCurrentOrder();
            }
        }
    }

    public void RefreshScreen()
    {
        Debug.Log("RefreshScreen called");

        if (screenRenderer == null)
            return;

        switch (gameFlow.currentState)
        {
            case GameState.Tutorial:

                if (
                    printer != null &&
                    printer.TutorialPagesFinished() &&
                    tutorialFinishedMaterial != null
                )
                {
                    screenRenderer.material =
                        tutorialFinishedMaterial;
                }
                else
                {
                    screenRenderer.material =
                        tutorialMaterial;
                }

                break;

            case GameState.ChoosingOrder:

                if (CurrentOrder != null)
                {
                    screenRenderer.material =
                        CurrentOrder.previewMaterial;
                }

                break;
        }
    }
    public void TestFinishCurrentTask()
    {
        Debug.Log(
            "TEST: Completing current state: " +
            gameFlow.currentState
        );

        if (gameFlow.currentState == GameState.Tutorial)
        {
            gameFlow.CompleteTutorial();
        }
        else if (gameFlow.currentState == GameState.ActiveOrder)
        {
            gameFlow.CompleteOrder();
        }

        RefreshScreen();
    }
}
