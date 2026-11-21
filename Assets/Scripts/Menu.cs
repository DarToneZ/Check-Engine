using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject[] highlight;
    public int highlightNum;
    [SerializeField] bool isLoopingList;
    [SerializeField] KeyCode nextKey;
    [SerializeField] KeyCode previousKey;
    [SerializeField] KeyCode altNextKey;
    [SerializeField] KeyCode altPreviousKey;
    [SerializeField] KeyCode pressKey;
    [SerializeField] KeyCode altPressKey;
    [SerializeField] KeyCode increaseVolumeKey;
    [SerializeField] KeyCode altIncreaseVolumeKey;
    [SerializeField] KeyCode decreaseVolumeKey;
    [SerializeField] KeyCode altDecreaseVolumeKey;
    [SerializeField] int NumberOfVolumeKey;

    [Header("Volume Slider")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] float volumeStep = 0.05f;

    [Header("Volume Key Repeat")]
    [SerializeField] float repeatDelay = 0.4f;
    [SerializeField] float repeatInterval = 0.08f;

    [Header("Music")]
    [SerializeField] AudioSource musicSource;   
    [SerializeField] float fadeOutDuration = 1.5f;

    [SerializeField] Animator SceneTransition;
    bool canChangeHighLight = true;

    float volume; 

    float increaseTimer;
    float decreaseTimer;
    bool increaseHeld;
    bool decreaseHeld;

    static AudioSource persistentMusic;

    private void Awake()
    {
        if (musicSource == null) return;

        if (persistentMusic != null && persistentMusic.gameObject != musicSource.gameObject)
        {
            Destroy(musicSource.gameObject);
            musicSource = persistentMusic;
            return;
        }

        persistentMusic = musicSource;
        DontDestroyOnLoad(musicSource.gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerPrefs.HasKey("volume"))
            volume = PlayerPrefs.GetInt("volume") / 100f;
        else
        {
            volume = 1f;
            PlayerPrefs.SetInt("volume", Mathf.RoundToInt(volume * 100f));
        }

        AudioListener.volume = volume;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        VolumeSliderUpdate();

        if (persistentMusic != null && !persistentMusic.isPlaying)
        {
            persistentMusic.loop = true;
            persistentMusic.Play();
        }
    }

    private void OnEnable()
    {
        CorrectHighlight();
    }

    public void CorrectHighlight()
    {
        for (int i = 0; i < highlight.Length; i++)
            highlight[i].SetActive(false);

        if (highlightNum >= highlight.Length)
        {
            if (isLoopingList) highlightNum = 0;
            else highlightNum--;
        }
        else if (highlightNum < 0)
        {
            if (isLoopingList) highlightNum = highlight.Length - 1;
            else highlightNum++;
        }

        highlight[highlightNum].SetActive(true);
        VolumeSliderUpdate();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(nextKey) || Input.GetKeyDown(altNextKey)) && canChangeHighLight)
        {
            highlightNum++;
            CorrectHighlight();
        }
        if ((Input.GetKeyDown(previousKey) || Input.GetKeyDown(altPreviousKey)) && canChangeHighLight)
        {
            highlightNum--;
            CorrectHighlight();
        }
        if (Input.GetKeyDown(pressKey) || Input.GetKeyDown(altPressKey))
        {
            buttons[highlightNum].onClick.Invoke();
        }

        if (highlightNum == NumberOfVolumeKey)
        {
            HandleVolumeKey(
                Input.GetKey(increaseVolumeKey) || Input.GetKey(altIncreaseVolumeKey),
                ref increaseHeld, ref increaseTimer,
                +volumeStep);
            HandleVolumeKey(
                Input.GetKey(decreaseVolumeKey) || Input.GetKey(altDecreaseVolumeKey),
                ref decreaseHeld, ref decreaseTimer,
                -volumeStep);
        }
        else
        {
            ResetVolumeKey(ref increaseHeld, ref increaseTimer);
            ResetVolumeKey(ref decreaseHeld, ref decreaseTimer);
        }
    }

    void HandleVolumeKey(bool isHeld, ref bool wasHeld, ref float timer, float delta)
    {
        if (!isHeld)
        {
            ResetVolumeKey(ref wasHeld, ref timer);
            return;
        }

        if (!wasHeld)
        {
            ApplyVolumeDelta(delta);
            wasHeld = true;
            timer = repeatDelay;
            return;
        }

        timer -= Time.unscaledDeltaTime;
        if (timer <= 0f)
        {
            ApplyVolumeDelta(delta);
            timer += repeatInterval;
            if (timer < 0f) timer = repeatInterval;
        }
    }

    void ResetVolumeKey(ref bool wasHeld, ref float timer)
    {
        wasHeld = false;
        timer = 0f;
    }

    void ApplyVolumeDelta(float delta)
    {
        float newVolume = Mathf.Clamp(volume + delta, 0f, 1f);
        newVolume = SnapToStep(newVolume);
        if (Mathf.Approximately(newVolume, volume)) return;
        volume = newVolume;
        SaveVolume();
        VolumeSliderUpdate();
        ApplyToGame();
    }

    float SnapToStep(float v)
    {
        if (volumeStep <= 0f) return v;
        return Mathf.Round(v / volumeStep) * volumeStep;
    }

    void SaveVolume()
    {
        PlayerPrefs.SetInt("volume", Mathf.RoundToInt(volume * 100f));
    }

    void ApplyToGame()
    {
        AudioListener.volume = volume;
    }

    void VolumeSliderUpdate()
    {
        if (volumeSlider == null) return;
        volumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        volume = SnapToStep(Mathf.Clamp01(value));
        SaveVolume();
        ApplyToGame();
    }

    public void LoadTo(int level)
    {
        canChangeHighLight = false;
        StartCoroutine(LoadScene(level, true));  
    }

    public void Credits(int level)
    {
        canChangeHighLight = false;
        StartCoroutine(LoadScene(level, false)); 
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator LoadScene(int scene, bool fadeMusic)
    {
        SceneTransition.SetTrigger("Start");

        if (fadeMusic && persistentMusic != null)
            yield return StartCoroutine(FadeOutAndDestroyMusic(fadeOutDuration));
        else
            yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeOutAndDestroyMusic(float duration)
    {
        AudioSource src = persistentMusic;
        float startVolume = src.volume;
        float t = 0f;

        while (t < duration && src != null)
        {
            t += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        if (src != null)
        {
            src.volume = 0f;
            src.Stop();
            Destroy(src.gameObject);
        }

        persistentMusic = null;
        musicSource = null;
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            print(PlayerPrefs.GetInt("level"));
            LoadTo(PlayerPrefs.GetInt("level"));
        }
        else
        {
            LoadTo(2);
        }
    }
}