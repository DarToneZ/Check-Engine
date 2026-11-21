using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] Animator SceneTransition;
    [SerializeField] float timeFadeIn;
    [SerializeField] float timeFadeOut;
    [SerializeField] Transform player;
    [SerializeField] Transform camera;
    Player playerScript;
    [SerializeField] Vector3 safePosition;

    private void Start()
    {
        playerScript = player.gameObject.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(OnDeath());
        }
    }
    IEnumerator OnDeath()
    {
        SceneTransition.speed = 1 / timeFadeIn;
        SceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(timeFadeIn);
        player.position = safePosition;
        camera.position = safePosition;
        player.rotation = Quaternion.Euler(0, 0, 0);
        playerScript.ResetPosition();
        SceneTransition.speed = 1 / timeFadeOut;
    }
}
