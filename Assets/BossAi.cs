using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

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
    private bool useGroundSmash; // Alternates between attacks
    public float smashAttackRange = 3f; // Range for the smash attack

    // Projectile Attack
    public float projectileAttackRange = 10f; // Range for the projectile attack
    private bool isCooldown = false; // Cooldown state after using projectile

    // Minimum distance for moving toward player (to avoid getting too close)
    public float minimumDistanceToPlayer = 2f;

    // States
    public float sightRange;
    public bool playerInSightRange;

    public Slider healthSlider;
    public GameObject healthBarUI;

    private void Awake()
    {
        player = GameObject.Find("player").transform;
        agent = GetComponent<NavMeshAgent>();

        // Set health slider's max value to the initial health
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health; // Set initial health value
        }
    }


    private void Update()
    {
        // Check for sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        bool playerInProjectileRange = Physics.CheckSphere(transform.position, projectileAttackRange, whatIsPlayer);
        bool playerInSmashRange = Physics.CheckSphere(transform.position, smashAttackRange, whatIsPlayer);

        // Move between patrol, chase, and attack states
        if (!playerInSightRange) Patroling();
        else if (playerInSightRange && !playerInProjectileRange) ChasePlayer();

        // Attack decision based on ranges
        if (!isCooldown)
        {
            if (playerInSmashRange)
            {
                AttackPlayerWithSmash();
            }
            else if (playerInProjectileRange)
            {
                AttackPlayerWithProjectile();
            }
        }
        else
        {
            // Move toward player after projectile attack (but respect minimum distance)
            if (Vector3.Distance(transform.position, player.position) > minimumDistanceToPlayer)
            {
                agent.SetDestination(player.position);
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
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

    private void ChasePlayer()
    {
        // Make sure the boss doesn't move too close
        if (Vector3.Distance(transform.position, player.position) > minimumDistanceToPlayer)
        {
            agent.SetDestination(player.position);
        }
    }

    private void AttackPlayerWithProjectile()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            FireProjectile();
            alreadyAttacked = true;
            isCooldown = true; // Start cooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void AttackPlayerWithSmash()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            StartCoroutine(GroundSmash());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void FireProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position + transform.forward * 2f, Quaternion.identity);
        Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        }
    }

    private System.Collections.IEnumerator GroundSmash()
    {
        // Optional: Trigger ground smash animation here
        Debug.Log("Ground Smash Attack!");

        yield return new WaitForSeconds(0.5f); // Delay for visual effect (match your animation)

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, smashRadius, smashDamageLayers);

        foreach (Collider hit in hitObjects)
        {
            // Assuming players or objects have a script with TakeDamage
            hit.GetComponent<PlayerStats>()?.TakeDamage(smashDamage);
        }

        // Optional: Add visual or sound effects for ground smash
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        isCooldown = false; // Reset cooldown after projectile attack
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Boss Health: " + health); // Add debug log for health

        if (healthBarUI != null && !healthBarUI.activeSelf)
        {
            healthBarUI.SetActive(true);
        }

        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }


    private void DestroyEnemy()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, projectileAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Smash radius visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, smashRadius);
    }
}
