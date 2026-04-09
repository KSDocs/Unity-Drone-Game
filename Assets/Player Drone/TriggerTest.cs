using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    [Header("Test Settings")]
    public string triggerMessage = "Trigger entered!";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(triggerMessage + " | Entered by: " + other.name);
    }
        
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Staying in trigger: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger: " + other.name);
    }
}