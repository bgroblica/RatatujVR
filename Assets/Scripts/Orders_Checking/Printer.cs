using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Printer : MonoBehaviour
{
    [Header("Refs")]
    public OrderManager orderManager;
    public Computer computer;

    [Header("Spawn Points")]
    public Transform spawnPoint;
    public Transform trayPoint;

    [Header("Printing")]
    public float printDuration = 0.6f;

    [Header("Tutorial Pages")]
    public List<GameObject> tutorialPages;

    private int currentTutorialPage = 0;

    private bool isPrinting = false;

    private const float happyThreshold = 0.5f;

    // ---------------- TUTORIAL ----------------

    public bool TutorialPagesFinished()
    {
        return currentTutorialPage >= tutorialPages.Count;
        if (TutorialPagesFinished())
        {
            if (computer != null)
            {
                computer.SetTutorialFinishedScreen();
            }
        }
    }

    public void PrintTutorialPage()
    {
        if (isPrinting)
            return;

        if (currentTutorialPage >= tutorialPages.Count)
        {
            Debug.Log("All tutorial pages printed.");
            return;
        }

        GameObject paper =
            Instantiate(
                tutorialPages[currentTutorialPage],
                spawnPoint.position,
                spawnPoint.rotation);

        StartCoroutine(MovePaperToTray(paper));

        currentTutorialPage++;

        Debug.Log(
            $"Printed tutorial page {currentTutorialPage}/{tutorialPages.Count}"
        );
    }

    // ---------------- ORDER ----------------

    public void PrintCurrentOrder()
    {
        if (isPrinting)
            return;

        ClientOrders order = orderManager.GetActiveOrder();

        if (order == null)
        {
            Debug.LogWarning("No active order!");
            return;
        }

        if (order.orderPaperPrefab == null)
        {
            Debug.LogError(
                $"Order {order.cakeName} has no order paper prefab!"
            );
            return;
        }

        GameObject paper =
            Instantiate(
                order.orderPaperPrefab,
                spawnPoint.position,
                spawnPoint.rotation);

        PrintedOrder printedOrder =
            paper.GetComponent<PrintedOrder>();

        if (printedOrder != null)
        {
            printedOrder.SetOrder(order);
        }

        StartCoroutine(MovePaperToTray(paper));
    }

    // ---------------- RESULT ----------------

    public void PrintResult(float score)
    {
        StartCoroutine(PrintResultDelayed(score));
    }

    private IEnumerator PrintResultDelayed(float score)
    {
        yield return new WaitForSeconds(1.5f);

        while (isPrinting)
        {
            yield return null;
        }

        ClientOrders order = orderManager.GetActiveOrder();

        if (order == null)
            yield break;

        GameObject prefab =
            score > happyThreshold
            ? order.happyResultPrefab
            : order.sadResultPrefab;

        if (prefab == null)
        {
            Debug.LogError(
                $"Missing result prefab on order {order.cakeName}"
            );
            yield break;
        }

        GameObject paper =
            Instantiate(
                prefab,
                spawnPoint.position,
                spawnPoint.rotation);

        PrintedResult result =
            paper.GetComponent<PrintedResult>();

        if (result != null)
        {
            result.SetScore(score);
        }

        StartCoroutine(MovePaperToTray(paper));
    }

    // ---------------- MOVEMENT ----------------

    private IEnumerator MovePaperToTray(GameObject paper)
    {
        isPrinting = true;

        Rigidbody rb = paper.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        XRGrabInteractable grab =
            paper.GetComponent<XRGrabInteractable>();

        if (grab != null)
        {
            grab.enabled = false;
        }

        Quaternion targetRotation =
            trayPoint.rotation *
            Quaternion.Euler(-90f, 0f, 0f);

        yield return new WaitForSeconds(0.2f);

        Vector3 startPos = spawnPoint.position;
        Vector3 endPos = trayPoint.position;

        Vector3 midPoint =
            (startPos + endPos) * 0.5f +
            Vector3.up * 0.15f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / printDuration;

            Vector3 a =
                Vector3.Lerp(startPos, midPoint, t);

            Vector3 b =
                Vector3.Lerp(midPoint, endPos, t);

            paper.transform.position =
                Vector3.Lerp(a, b, t);

            paper.transform.rotation = targetRotation;

            yield return null;
        }

        paper.transform.position = endPos;
        paper.transform.rotation = targetRotation;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (grab != null)
        {
            grab.enabled = true;
        }

        isPrinting = false;
    }
}