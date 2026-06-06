using UnityEngine;

public class PrintedResult : MonoBehaviour
{
    [Header("Star visuals")]
    public GameObject[] stars; // 5 objects

    public void SetStars(int rating)
    {
        rating = Mathf.Clamp(rating, 0, stars.Length);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < rating);
        }

        Debug.Log("Printed result: " + rating + " stars");
    }
}