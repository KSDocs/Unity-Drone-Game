using UnityEngine;
using UnityEngine.Events;

public class ShockReceiver : MonoBehaviour, IShockInteractable
{
    [Header("Shock Events")]
    public UnityEvent onShock;   // assign in Inspector

    // Called by your laser system
    public void OnShockHit()
    {
        onShock?.Invoke();
    }

    // Optional debug function (you can hook this in the event)
    public void DebugHit()
    {
        Debug.Log("HIT: " + gameObject.name);
    }
}