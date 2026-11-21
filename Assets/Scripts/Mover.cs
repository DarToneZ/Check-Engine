using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Vector3 speed;
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            transform.position += speed * Time.fixedDeltaTime; //it's work nice with less Fixed Time Step, but it brokes all other physics
        }
    }
}
