using UnityEngine;

public class DroneShoot : MonoBehaviour
{
    [Header("Laser")]
    public float range = 50f;
    public LineRenderer laser;
    public Transform firePoint;

    [Header("Cooldown")]
    public float shockCooldown = 2f;
    private float cooldownTimer = 0f;

    [Header("Laser Burst")]
    public float laserDuration = 0.1f;
    private float laserTimer = 0f;

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
        }
    }

    void HandleLaser()
    {
        if (laserTimer > 0f)
        {
            laserTimer -= Time.deltaTime;
        }
        else
        {
            laser.enabled = false;
        }
    }
    void HandleCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f) cooldownTimer = 0f;
        }

        // Update UI every frame
        if (cooldownUI != null)
        {
            float progress = cooldownTimer / shockCooldown; // 1 = full, 0 = ready
            cooldownUI.SetCooldownProgress(progress);
        }
    }


    void FireLaser()
    {
        Debug.Log("shoot");

        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            endPoint = hit.point;

            IShockInteractable reactable =
                hit.collider.GetComponentInParent<IShockInteractable>();

            if (reactable != null)
            {
                reactable.OnShockHit();
            }
        }
        else
        {
            endPoint = firePoint.position + firePoint.forward * range;
        }

        laser.enabled = true;
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, endPoint);
    }
}