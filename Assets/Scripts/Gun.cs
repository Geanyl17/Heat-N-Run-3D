using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 100; // Maximum ammo capacity
    public int currentAmmo;   // Current ammo count
    public int ammoPerReload = 30; // Ammo replenished per reload
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;

    public AmmoUIManager ammoUIManager; // Reference to the AmmoUIManager to update UI

    void Start()
    {
        currentAmmo = maxAmmo; // Set initial ammo to max ammo
        if (ammoUIManager != null)
        {
            ammoUIManager.SetCurrentWeapon(this); // Set the player gun in the AmmoUIManager
        }
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
        {
            Shoot();
        }

        // Reload ammo when pressing the reload key (e.g., R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--; // Decrease ammo on each shot
            if (ammoUIManager != null)
            {
                ammoUIManager.UpdateAmmoDisplay(currentAmmo, maxAmmo); // Update ammo UI
            }

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                // Check if the hit object has the EnemyAiTutorial component
                EnemyAiTutorial enemy = hit.transform.GetComponent<EnemyAiTutorial>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Call TakeDamage on the enemy
                    Debug.Log("Hit enemy and dealt damage!");
                }
                else
                {
                    // Check for target
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);    
                    }
                }
            }
        }
        else
        {
            Debug.Log("Out of Ammo!");
        }
    }

    public void Reload()
    {
        currentAmmo = ammoPerReload;
        if (ammoUIManager != null)
        {
            ammoUIManager.UpdateAmmoDisplay(currentAmmo, maxAmmo); // Update ammo UI after reload
        }
        Debug.Log("Reloading...");
    }

    public void ReplenishAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo); // Ensure ammo doesn't exceed the max limit
        if (ammoUIManager != null)
        {
            ammoUIManager.UpdateAmmoDisplay(currentAmmo, maxAmmo); // Update ammo UI after replenishment
        }
        Debug.Log($"Ammo replenished! Current ammo: {currentAmmo}");
    }
}
