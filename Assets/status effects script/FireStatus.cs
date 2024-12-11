using UnityEngine;
using UnityEngine.Audio; // Needed for AudioMixer
using UnityEngine.UI;

public class FireStatus : MonoBehaviour
{
    public float damageAmount = 5f;
    public float duration = 3f;
    public float damageInterval = 1f; // Interval between damage applications
    public GameObject fireParticlePrefab; // Reference to the fire particle prefab
    public AudioClip fireSound; // Reference to the fire burning sound
    public Image fireIndicator; // UI Image for the on-fire effect
    public AudioMixerGroup audioMixerGroup; // Reference to the AudioMixerGroup for routing sound through the mixer

    private GameObject activeFireParticle; // To track the particle instance
    private AudioSource audioSource; // Audio source for sound effects

    private void Start()
    {
        // Ensure there is an AudioSource component on this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Set the AudioSource to use the provided AudioMixerGroup
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            StartCoroutine(ApplyDamageOverTime(playerStats));
        }
    }

    private System.Collections.IEnumerator ApplyDamageOverTime(PlayerStats playerStats)
    {
        float elapsed = 0f;

        // Spawn fire particles attached to the player's camera
        if (fireParticlePrefab != null && activeFireParticle == null)
        {
            Camera playerCamera = Camera.main; // Assuming the player's camera is tagged as "MainCamera"

            if (playerCamera != null)
            {
                activeFireParticle = Instantiate(fireParticlePrefab, playerCamera.transform);
                activeFireParticle.transform.localPosition = new Vector3(0, 0, 1); // Offset 2 units in front of the camera
                activeFireParticle.transform.localRotation = Quaternion.identity; // Reset rotation
            }
            else
            {
                Debug.LogWarning("No MainCamera found! Fire particles might not be visible.");
            }
        }

        // Start playing fire sound
        if (fireSound != null)
        {
            audioSource.clip = fireSound;
            audioSource.Play();
        }

        // Show fire indicator
        if (fireIndicator != null)
        {
            StartCoroutine(FadeInFireIndicator());
        }

        // Apply damage over time
        while (elapsed < duration)
        {
            playerStats.TakeDamage(damageAmount);
            elapsed += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

        // Stop the fire effect
        if (activeFireParticle != null)
        {
            Destroy(activeFireParticle);
        }

        // Stop playing sound
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Fade out the fire indicator
        if (fireIndicator != null)
        {
            StartCoroutine(FadeOutFireIndicator());
        }
    }

    private System.Collections.IEnumerator FadeInFireIndicator()
    {
        float fadeDuration = 0.5f; // Duration for fade in
        float elapsed = 0f;

        Color color = fireIndicator.color;
        color.a = 0; // Start transparent
        fireIndicator.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fireIndicator.color = color;
            yield return null;
        }
    }

    private System.Collections.IEnumerator FadeOutFireIndicator()
    {
        float fadeDuration = 0.5f; // Duration for fade out
        float elapsed = 0f;

        Color color = fireIndicator.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            fireIndicator.color = color;
            yield return null;
        }
    }
}
