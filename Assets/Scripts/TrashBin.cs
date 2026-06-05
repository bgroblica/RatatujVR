using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null && other.transform.root == transform.root)
            return;

        Debug.Log($"Destroyed by trash bin: {other.name}");

        Destroy(other.transform.root.gameObject);
    }
}