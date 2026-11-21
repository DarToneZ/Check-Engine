using UnityEngine;
using TMPro;
using System.Collections;

public class DefaultText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text textToSHOW;
    [SerializeField] float delay;
    [SerializeField] float showingDelay;
    [SerializeField] bool showOnStart = true;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        if(showOnStart)
        {
            ShowText(textToSHOW, delay, showingDelay);
        }
    }

    void ShowText(TMP_Text textToShow, float delay, float showingDelay)
    {
        StartCoroutine(ShowTextIE(textToShow, delay, showingDelay));
    }

    IEnumerator ShowTextIE(TMP_Text textToShow, float delay, float showingDelay)
    {
        text.text = null;
        yield return new WaitForSeconds(showingDelay);
        for (int i = 0; i < textToShow.text.Length; i++)
        {
            text.text += textToShow.text[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
