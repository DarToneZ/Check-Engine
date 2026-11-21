using UnityEngine;

public class LoopingScroller : MonoBehaviour
{
    public float speed = 2f;

    public float leftBound = -30f;

    public float teleportOffset = 60f;

    public bool moveLeft = true;

    void Update()
    {
        float direction = moveLeft ? -1f : 1f;
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        float x = transform.position.x;

        if (moveLeft && x < leftBound)
        {
            transform.position += Vector3.right * teleportOffset;
        }
        else if (!moveLeft && x > leftBound)
        {
            transform.position -= Vector3.right * teleportOffset;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(leftBound, transform.position.y - 5f, 0),
            new Vector3(leftBound, transform.position.y + 5f, 0)
        );

        Gizmos.color = Color.green;
        Vector3 teleportPos = new Vector3(leftBound + (moveLeft ? teleportOffset : -teleportOffset), transform.position.y, 0);
        Gizmos.DrawLine(
            new Vector3(teleportPos.x, transform.position.y - 5f, 0),
            new Vector3(teleportPos.x, transform.position.y + 5f, 0)
        );
    }
}