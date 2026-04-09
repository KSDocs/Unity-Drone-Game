using UnityEngine;
using UnityEngine.Events;

public class DroneEventGenInteraction : MonoBehaviour, IShockInteractable
{
    public UnityEvent onZap;

    public void OnShockHit()
    {
        onZap?.Invoke();
    }

}
