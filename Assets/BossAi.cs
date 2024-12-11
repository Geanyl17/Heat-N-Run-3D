using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public GameObject victoryScreen;
    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // Ground Smash Attack
    public float smashRadius = 5f;
    public float smashDamage = 20f;
    public LayerMask smashDamageLayers;
    public float smashAttackRange = 3f;

    // Projectile Attack
    public float projectileAttackRange = 10f;
    private bool isCooldown = false;

    // Minimum distance for moving toward player
    public float minimumDistanceToPlayer = 2f;

    // States
    public float sightRange;
    public bool playerInSightRange;

    public Slider healthSlider;
    public GameObject healthBarUI;

    public Animator animator;

    private void Awake()
    {
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();

        // Set health slider's max value to the initial health
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    private void Update()
    {
        // Check for ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // Manage the boss's states
        ManageStates();
    }

    private void ManageStates()
    {
        if (!playerInSightRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !isCooldown)
        {
            if (Physics.CheckSphere(transform.position, smashAttackRange, whatIsPlayer))
                AttackPlayerWithSmash();
            else if (Physics.CheckSphere(transform.position, projectileAttackRange, whatIsPlayer))
                AttackPlayerWithProjectile();
            else
                MoveTowardPlayer();
        }
        else
        {
            MoveTowardPlayer(); // Default movement during cooldown
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isWalking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            animator.SetBool("isWalking", false);
        }
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void MoveTowardPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > minimumDistanceToPlayer)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
        }
    }

    private void AttackPlayerWithProjectile()
    {
        agent.SetDestination(transform.position); // Stop moving
        agent.velocity = Vector3.zero;
        transform.LookAt(player); // Face the player

        if (!alreadyAttacked)
        {
            animator.SetTrigger("isThrowing");
            StartCoroutine(DelayedProjectileAttack());
            StartCooldown();
        }
    }

    private System.Collections.IEnumerator DelayedProjectileAttack()
    {
        agent.enabled = false; // Disable movement
        yield return new WaitForSeconds(2f); // Adjust to match animation timing

        FireProjectile();

        yield return new WaitForSeconds(0.5f); // Small buffer to allow animation completion
        agent.enabled = true; // Re-enable movement
    }


    private void AttackPlayerWithSmash()
    {
        agent.SetDestination(transform.position); // Stop moving
        agent.velocity = Vector3.zero;
        transform.LookAt(player); // Face the player

        if (!alreadyAttacked)
        {
            animator.SetTrigger("isSmashing");
            StartCoroutine(DelayedSmashAttack());
            StartCooldown();
        }
    }

    private System.Collections.IEnumerator DelayedSmashAttack()
    {
        agent.enabled = false; // Disable movement
        yield return new WaitForSeconds(1.8f); // Adjust to match animation timing

        StartCoroutine(GroundSmash());

        yield return new WaitForSeconds(0.5f); // Small buffer to allow animation completion
        agent.enabled = true; // Re-enable movement
    }


    private void FireProjectile()
    {
        // Check if the player is stationary
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        Vector3 playerVelocity = playerRb != null ? playerRb.velocity : Vector3.zero;

        // Calculate the target position
        Vector3 targetPosition;
        if (playerVelocity.magnitude < 0.1f) // If the player is almost stationary
        {
            targetPosition = player.position;
        }
        else
        {
            // Predict player's future position
            float projectileSpeed = 32f; // Match the force applied to the projectile
            Vector3 toPlayer = player.position - transform.position;
            float timeToHit = toPlayer.magnitude / projectileSpeed;
            targetPosition = player.position + playerVelocity * timeToHit;
        }

        // Calculate aim direction
        Vector3 aimDirection = (targetPosition - transform.position).normalized;

        // Spawn and fire the projectile
        GameObject projectileInstance = Instantiate(projectile, transform.position + transform.forward * 2f, Quaternion.identity);
        Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = aimDirection * 32f; // Apply speed
        }
}


    private System.Collections.IEnumerator GroundSmash()
    {
        Debug.Log("Ground Smash Attack!");

        yield return new WaitForSeconds(0.5f); // Delay for visual effect (match your animation)

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, smashRadius, smashDamageLayers);

        foreach (Collider hit in hitObjects)
        {
            hit.GetComponent<PlayerStats>()?.TakeDamage(smashDamage);
        }
    }

    private void StartCooldown()
    {
        alreadyAttacked = true;
        isCooldown = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        isCooldown = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);

        animator.SetTrigger("isHurt");

        if (healthBarUI != null && !healthBarUI.activeSelf)
        {
            healthBarUI.SetActive(true);
        }

        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        if (health <= 0)
        {
            // If the object is a boss, trigger the victory screen
            if (CompareTag("Boss")) // Check if the tag is "Boss"
            {
                Invoke(nameof(DestroyEnemy), 0.5f);
            }
            else
            {
                // Destroy non-boss enemies directly without showing the victory screen
                DestroyEnemy();
            }
        }
    }

    private void DestroyEnemy()
    {
        if (CompareTag("Boss") && victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            StartCoroutine(LoadSceneWithDelay(10f));
        }

        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }

        Destroy(gameObject); // Optional: Destroy the object after scene load
    }

    private System.Collections.IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("VictoryScene");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, projectileAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, smashRadius);
    }
}
