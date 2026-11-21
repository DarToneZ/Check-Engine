using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPTypewriter : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private string fullText;

    [SerializeField] private float charsPerSecond = 30f;
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float punctuationDelay = 0.3f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.7f;
    [SerializeField] private bool randomPitch = true;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private bool soundOnWhitespace = false;

    [SerializeField] private bool playOnStart = true;

    private TMP_Text tmpText;
    private Coroutine typingCoroutine;

    public bool IsTyping { get; private set; }

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    private void Start()
    {
        if (playOnStart)
            StartTyping(fullText);
    }

    public void StartTyping(string text)
    {
        fullText = text;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeRoutine());
    }

    public void StartTyping() => StartTyping(fullText);

    private IEnumerator TypeRoutine()
    {
        IsTyping = true;

        tmpText.text = fullText;
        tmpText.ForceMeshUpdate();

        int totalVisible = tmpText.textInfo.characterCount;
        tmpText.maxVisibleCharacters = 0;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float baseDelay = 1f / Mathf.Max(0.01f, charsPerSecond);

        for (int i = 1; i <= totalVisible; i++)
        {
            tmpText.maxVisibleCharacters = i;

            char c = tmpText.textInfo.characterInfo[i - 1].character;

            bool isWhitespace = char.IsWhiteSpace(c);
            if (typeSound != null && (!isWhitespace || soundOnWhitespace))
                PlayTypeSound();

            float delay = baseDelay;
            if (c == '.' || c == '!' || c == '?')
                delay += punctuationDelay;
            else if (c == ',' || c == ';' || c == ':')
                delay += punctuationDelay * 0.5f;
            else if (c == '\n' || c == '\r')
                delay += punctuationDelay;

            yield return new WaitForSeconds(delay);
        }

        IsTyping = false;
    }

    private void PlayTypeSound()
    {
        if (audioSource == null || typeSound == null) return;

        if (randomPitch)
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(typeSound, volume);
    }
}