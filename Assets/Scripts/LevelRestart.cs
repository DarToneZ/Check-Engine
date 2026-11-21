using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRestart : MonoBehaviour
{
    [SerializeField] CanvasGroup blackBox;
    [SerializeField] float timeToRestart;
    float holdedTime;
    bool isHolding;

    private void Update()
    {
        blackBox.alpha = holdedTime / timeToRestart;
        if(Input.GetKeyDown(KeyCode.R))
        {
            isHolding = true;
        }
        if(Input.GetKey(KeyCode.R) & isHolding)
        {
            holdedTime += Time.deltaTime;
            if(holdedTime > timeToRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (holdedTime > 0)
        {
            holdedTime -= Time.deltaTime;
        }
    }
}
