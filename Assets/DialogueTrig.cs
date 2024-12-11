using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueTrigger : MonoBehaviour
{
    public AudioClip dialogueAudioClip; // Audio to play with dialogue
    public string dialogueText; // The dialogue to display
    public AudioSource audioSource; // Reference to Audio Source
    public TextMeshProUGUI dialogueTextComponent; // Text component for dialogue
    public GameObject dialogueBox; // The UI panel for the dialogue box
    public float typingSpeed = 0.05f; // Speed for the typing effect
    public float textDisplayDuration = 3f; // Time to keep the text visible after typing ends

    private bool hasTriggered = false; // Ensure the trigger works once

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Prevent retriggering
            StartCoroutine(ShowDialogue());
        }
    }

    private IEnumerator ShowDialogue()
    {
        // Enable the dialogue box
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
        }

        // Play the dialogue audio
        if (audioSource != null && dialogueAudioClip != null)
        {
            audioSource.PlayOneShot(dialogueAudioClip);
        }

        // Display the text with a typing effect
        dialogueTextComponent.text = ""; // Clear any previous text
        foreach (char letter in dialogueText.ToCharArray())
        {
            dialogueTextComponent.text += letter; // Append one character at a time
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for the specified duration after typing ends
        yield return new WaitForSeconds(textDisplayDuration);

        // Disable the dialogue box
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }
}
