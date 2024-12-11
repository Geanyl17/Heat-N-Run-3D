using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public HealthBar healthBar;
    private float currentHealth;
    public PlayerMovement playerMovement;
    public GameObject deathScreen;
    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
    }
    private void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
    }
    public void HealPlayer(float amount)
    {
        currentHealth += amount;
        healthBar.SetSlider(currentHealth);
    }
    private void Die()
    {
        Debug.Log("You died!");

        deathScreen.SetActive(true);

        Time.timeScale = 0f;

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;  // Unpause the game
        deathScreen.SetActive(false); // Deactivate the death screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;  // Unpause the game
        deathScreen.SetActive(false); // Deactivate the death screen
        SceneManager.LoadScene("Menu"); // Load the main menu scene
    }

}