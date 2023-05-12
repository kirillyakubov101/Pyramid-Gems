using UnityEngine;
using System;

public class Collector : MonoBehaviour
{
    public static event Action OnGemCollected;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        OnGemCollected?.Invoke();
    }
}
