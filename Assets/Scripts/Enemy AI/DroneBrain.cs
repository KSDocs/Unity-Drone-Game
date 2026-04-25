using UnityEngine;

public class DroneBrain : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    [Range(0, 180)] public float viewAngle = 45f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("References")]
    public PatrolNavMesh patrol;
    public Transform eyePoint;

    private bool isDetected;

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Collider[] targets = Physics.OverlapSphere(
            eyePoint.position,
            detectionRadius,
            playerMask
        );

        bool found = false;

        foreach (Collider target in targets)
        {
            Transform player = target.transform;

            Vector3 dir = (player.position - eyePoint.position).normalized;

            float angle = Vector3.Angle(eyePoint.forward, dir);

            if (angle < viewAngle)
            {
                float dist = Vector3.Distance(eyePoint.position, player.position);

                if (!Physics.Raycast(eyePoint.position, dir, dist, obstacleMask))
                {
                    found = true;
                    break;
                }
            }
        }

        ApplyState(found);
    }

    void ApplyState(bool detected)
    {
        if (detected)
        {
            if (!isDetected)
                Debug.Log("Player Detected in Cone Vision!");

            isDetected = true;

            if (patrol != null)
                patrol.Stop();
        }
        else
        {
            isDetected = false;

            if (patrol != null)
                patrol.Resume();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (eyePoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyePoint.position, detectionRadius);

        Vector3 forward = eyePoint.forward;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle, 0) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyePoint.position, leftBoundary * detectionRadius);
        Gizmos.DrawRay(eyePoint.position, rightBoundary * detectionRadius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.15f);

        int steps = 25;
        float stepAngle = (viewAngle * 2f) / steps;

        Vector3 lastPoint = eyePoint.position +
            Quaternion.Euler(0, -viewAngle, 0) * forward * detectionRadius;

        for (int i = 1; i <= steps; i++)
        {
            float angle = -viewAngle + stepAngle * i;

            Vector3 nextPoint = eyePoint.position +
                Quaternion.Euler(0, angle, 0) * forward * detectionRadius;

            Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }
}