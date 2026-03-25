using UnityEngine;

public class Spoon : MonoBehaviour
{
    private Vector3 lastPosition;
    public float smoothedSpeed;
    private float smoothFactor = 10f;

    void Update()
    {
        float rawSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        smoothedSpeed = Mathf.Lerp(smoothedSpeed, rawSpeed, Time.deltaTime * smoothFactor);

        lastPosition = transform.position;
    }
}