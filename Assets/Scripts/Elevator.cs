using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Collider2D activateCheck;
    [SerializeField] Collider2D deactivateCheck;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Collider2D playerWheel1Collider;
    [SerializeField] Collider2D playerWheel2Collider;

    [SerializeField] GameObject player;
    [SerializeField] float speed;
    [SerializeField] float minPosition;
    [SerializeField] float maxPosition;
    Rigidbody2D rb;
    [SerializeField] GameObject shesterenka;
    [SerializeField] float shesterenkaRotatingSpeed;
    [SerializeField] bool onlyUp = false;

    [SerializeField] bool horizontal;

    [SerializeField] float delay = 0.5f;
    float timeOn;

    Rigidbody2D playerRB;
    [SerializeField] float velocityBoost;
    bool activated;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(activateCheck.IsTouching(playerWheel1Collider) & activateCheck.IsTouching(playerWheel2Collider))
        {
            timeOn += Time.fixedDeltaTime;
            if(rb.position.y < maxPosition && timeOn > delay && !horizontal)
            {
                rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);
                shesterenka.transform.Rotate(new Vector3(0f, 0f, shesterenkaRotatingSpeed * Time.fixedDeltaTime));
            }
            if(rb.position.x < maxPosition && timeOn > delay && horizontal)
            {
                //player.transform.SetParent(gameObject.transform);
                rb.MovePosition(rb.position + Vector2.right * speed * Time.fixedDeltaTime);
                //playerRB.MovePosition(playerRB.position + Vector2.right * speed.x * Time.fixedDeltaTime);
                //player.transform.position = player.transform.position + Vector3.right * speed.x * Time.fixedDeltaTime;
                //player.transform.Translate(Vector3.right * speed.x * Time.fixedDeltaTime);
                shesterenka.transform.Rotate(new Vector3(0f, 0f, shesterenkaRotatingSpeed * Time.fixedDeltaTime));
                //playerRB.MovePosition(playerRB.position + deltaMovement);
                //player.transform.GetComponent<Player>().MoveWithPlatform(deltaMovement);
                if(!activated)
                {
                    playerRB.linearVelocity = playerRB.linearVelocity + Vector2.right * velocityBoost;
                }
                activated = true;
            }
        }
        else if (!(deactivateCheck.IsTouching(playerWheel1Collider) || deactivateCheck.IsTouching(playerWheel2Collider)))
        {
            activated = false;
            timeOn = 0;
            if (rb.position.y > minPosition & !onlyUp && !horizontal)
            {
                rb.MovePosition(rb.position - Vector2.up * speed * Time.fixedDeltaTime);
                shesterenka.transform.Rotate(new Vector3(0f, 0f, -shesterenkaRotatingSpeed * Time.fixedDeltaTime));
            }
            if (rb.position.x > minPosition & !onlyUp && horizontal)
            {
                rb.MovePosition(rb.position - Vector2.right * speed * Time.fixedDeltaTime);
                shesterenka.transform.Rotate(new Vector3(0f, 0f, -shesterenkaRotatingSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
