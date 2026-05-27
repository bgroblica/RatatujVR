using UnityEngine;

public class SnapSocket : MonoBehaviour
{
    public SnapType socketType;

    // instead of bool (THIS is the key fix)
    public Transform currentOccupant;

    public bool IsFree()
    {
        return currentOccupant == null;
    }

    public void Occupy(Transform obj)
    {
        currentOccupant = obj;
    }

    public void Clear()
    {
        currentOccupant = null;
    }
}