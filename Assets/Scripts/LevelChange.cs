using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] Animator SceneTransition;
    [SerializeField] int nextLevel;
    [SerializeField] int time;

    private void Start()
    {
        SceneTransition.speed = 1f;
        SceneTransition.Play("SceneTransitionEnd", 0, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadScene(nextLevel, time));
            PlayerPrefs.SetInt("progress", nextLevel);
            print("next");
        }
    }
    IEnumerator LoadScene(int scene, float time)
    {
        SceneTransition.speed = 1 / time;
        SceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }
}
