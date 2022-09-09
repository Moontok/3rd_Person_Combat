using System;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;

    [field: SerializeField] public Transform HangPoint { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
    }
}
