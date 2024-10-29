using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
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
                // If you still want to check for Target component
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);    
                }
            }
        }
    }
}
