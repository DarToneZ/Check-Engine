using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    bool isPaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject[] hideInPause; //only for things that can't be on/off other way
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Continue();
            }
            else
            {
                Time.timeScale = 0f;
                Menu menu = pauseMenu.GetComponent<Menu>();
                menu.highlightNum = 0;
                menu.CorrectHighlight();
                isPaused = true;
                pauseMenu.SetActive(true);
                for (int i = 0; i < hideInPause.Length; i++)
                {
                    hideInPause[i].SetActive(false);
                }
            }
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
        for (int i = 0; i < hideInPause.Length; i++)
        {
            hideInPause[i].SetActive(true);
        }
    }

    public void LoadTo(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
