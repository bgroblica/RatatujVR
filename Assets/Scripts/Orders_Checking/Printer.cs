using System.Collections;
using UnityEngine;

public class Printer : MonoBehaviour
{
    [Header("Refs")]
    public OrderManager orderManager;

    [Header("Prefabs")]
    public GameObject orderPaperPrefab;
    public GameObject resultPaperPrefab;

    public Transform printPoint;

    // ---------------- ORDER PRINT ----------------
    public void PrintCurrentOrder()
    {
        ClientOrders order = orderManager.GetActiveOrder();

        if (order == null)
        {
            Debug.LogWarning("No active order to print!");
            return;
        }

        GameObject paper =
            Instantiate(orderPaperPrefab, printPoint.position, printPoint.rotation);

        PrintedOrder po = paper.GetComponent<PrintedOrder>();

        if (po != null)
            po.SetOrder(order);

        Debug.Log("Printed order: " + order.cakeName);
    }

    // ---------------- RESULT PRINT ----------------
    public void PrintResult(int stars)
    {
        StartCoroutine(PrintResultDelayed(stars));
    }

    private IEnumerator PrintResultDelayed(int stars)
    {
        yield return new WaitForSeconds(1.5f);

        GameObject paper =
            Instantiate(resultPaperPrefab, printPoint.position, printPoint.rotation);

        PrintedResult pr = paper.GetComponent<PrintedResult>();

        if (pr != null)
            pr.SetStars(stars);
    }
}