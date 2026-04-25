using UnityEngine;

public class DroneShoot : MonoBehaviour
{
    [Header("Laser")]
    public float range = 50f;
    public LineRenderer laser;
    public Transform firePoint;
    public LayerMask hitMask = ~0;

    [Header("Cooldown")]
    public float shockCooldown = 2f;
    private float cooldownTimer;

    [Header("Laser Burst")]
    public float laserDuration = 0.1f;
    private float laserTimer;

    [Header("UI")]
    public ShockCooldownUI cooldownUI;

    void Update()
    {
        HandleCooldown();
        HandleShooting();
        HandleLaser();
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            FireLaser();
            cooldownTimer = shockCooldown;
            laserTimer = laserDuration;

            if (cooldownUI != null)
                cooldownUI.OnCooldownStarted();
        }
    }

    void HandleCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f)
                cooldownTimer = 0f;
        }

        if (cooldownUI != null)
        {
            float progress = 1f - (cooldownTimer / shockCooldown);
            progress = Mathf.Clamp01(progress);
            cooldownUI.SetCooldownProgress(progress);
        }
    }

    void HandleLaser()
    {
        if (laserTimer > 0f)
        {
            laserTimer -= Time.deltaTime;
        }

        if (laser != null)
        {
            laser.enabled = laserTimer > 0f;
        }
    }

    void FireLaser()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Vector3 endPoint = firePoint.position + firePoint.forward * range;

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
        {
            endPoint = hit.point;

            IShockInteractable reactable = hit.collider.GetComponentInParent<IShockInteractable>();
            if (reactable != null)
            {
                reactable.OnShockHit();
            }
        }

        if (laser != null)
        {
            laser.enabled = true;
            laser.SetPosition(0, firePoint.position);
            laser.SetPosition(1, endPoint);
        }
    }
}