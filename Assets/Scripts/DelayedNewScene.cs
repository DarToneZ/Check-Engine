using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedNewScene : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] int nextScene;
    float time = 0f;
    void Update()
    {
        time += Time.deltaTime;
        if(time > delay)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
