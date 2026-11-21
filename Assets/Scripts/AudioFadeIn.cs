using System.Collections;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float targetVolume = 1f;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] bool oneTime = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !(oneTime && hasTriggered))
        {
            hasTriggered = true;
            StartCoroutine(FadeIn(targetVolume, fadeDuration));
        }
    }

    IEnumerator FadeIn(float target, float duration)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, target, elapsed / duration);
            yield return null;
        }

        audioSource.volume = target;
    }
}