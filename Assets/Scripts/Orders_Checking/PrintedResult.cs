using UnityEngine;

public class PrintedResult : MonoBehaviour
{
    public Transform fillBar;
    public float maxXScale = 1f;

    public void SetScore(float score)
    {
        score = Mathf.Clamp01(score);

        Vector3 scale = fillBar.localScale;
        scale.x = maxXScale * score;
        fillBar.localScale = scale;

        Debug.Log("Printed score: " + score);
    }
}