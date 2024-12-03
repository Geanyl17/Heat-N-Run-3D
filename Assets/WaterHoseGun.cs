using System.Collections; // Required for IEnumerator
using UnityEngine;

public class WaterHoseGun : Gun
{
    public GameObject waterSprayPrefab; // Prefab reference
    private ParticleSystem waterSprayEffect; // Particle system instance

    public float damageInterval = 0.2f; // Time interval between damage applications
    public float damageAmount = 10f;   // Damage amount
    private float nextDamageTime;      // Time until the next damage can be applied
    public float ammoConsumptionRate = 5f; // Ammo units consumed per interval


    private Coroutine damageCoroutine;

    void Start()
    {
        base.Start();

        if (waterSprayPrefab != null)
        {
            if (waterSprayEffect != null)
                waterSprayEffect.Stop();
        }
        else
        {
            Debug.LogError("Water Spray Prefab not assigned!");
        }
    }

    void Update()
    {
        base.Update();

        if (Input.GetButton("Fire1") && currentAmmo > 0)
        {
            if (damageCoroutine == null)
            {
                if (waterSprayEffect != null && !waterSprayEffect.isPlaying)
                {
                    waterSprayEffect.Play();
                }
                damageCoroutine = StartCoroutine(ApplyDamageOverTime());
            }
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            if (waterSprayEffect != null && waterSprayEffect.isPlaying)
            {
                waterSprayEffect.Stop();
            }
        }
    }

    IEnumerator ApplyDamageOverTime()
    {
        while (currentAmmo > 0)
        {
            if (Time.time >= nextDamageTime)
            {
                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 100f))
                {
                    EnemyAiTutorial enemy = hit.transform.GetComponent<EnemyAiTutorial>();
                    BossAi boss = hit.transform.GetComponent<BossAi>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damageAmount);
                        Debug.Log("Hit enemy and dealt damage!");
                    }
                    else if (boss != null)
                    {
                        boss.TakeDamage(damageAmount);
                        Debug.Log("Hit boss and dealt damage!");
                    }
                    else
                    {
                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            target.TakeDamage(damageAmount);
                        }
                    }
                }

                currentAmmo -= Mathf.CeilToInt(ammoConsumptionRate * damageInterval);
                if (currentAmmo < 0) currentAmmo = 0;

                if (ammoUIManager != null)
                {
                    ammoUIManager.UpdateAmmoDisplay(currentAmmo, maxAmmo);
                }

                nextDamageTime = Time.time + damageInterval;
            }

            yield return null; // Wait for the next frame
        }

        Debug.Log("Out of Ammo!");

        if (waterSprayEffect != null && waterSprayEffect.isPlaying)
        {
            waterSprayEffect.Stop();
        }
    }
}
