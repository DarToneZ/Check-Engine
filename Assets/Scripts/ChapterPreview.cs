using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterPreview : MonoBehaviour
{
    [SerializeField] Animator SceneTransition;
    [SerializeField] int nextLevel;
    [SerializeField] int timeToNext;
    [SerializeField] int animationTime;
    float time;

    private void Update()
    {
        time += Time.deltaTime;
        if(time > timeToNext)
        {
            StartCoroutine(LoadScene(nextLevel, animationTime));
        }
    }

    IEnumerator LoadScene(int scene, float animationTime)
    {
        SceneTransition.speed = 1 / animationTime;
        SceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene(scene);
    }
}
