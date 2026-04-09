using UnityEngine;
using UnityEngine.Events;
public class ShockEvent : MonoBehaviour, IShockInteractable
{
    [Header("Shock Event")]
    public UnityEvent onShock;

    [Header("Debug")]
    public bool debugLog = true;

    public void OnShockHit()
    {
        if (debugLog)
            Debug.Log(name + " was shocked!");

        onShock?.Invoke();
    }
}