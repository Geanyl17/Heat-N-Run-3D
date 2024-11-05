using System.Collections;
using UnityEngine;

public class BlindScript : MonoBehaviour
{
    public GameObject Blind;

    private void OnTriggerEnter(Collider other)
    {
        Blind.SetActive(true);  
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(DisableBlindAfterDelay(5f));
    }

    private IEnumerator DisableBlindAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Blind.SetActive(false);
    }
}
