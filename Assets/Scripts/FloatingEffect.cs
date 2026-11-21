using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Header("Параметры плавания")]
    [SerializeField] private float amplitudeY = 10f;      
    [SerializeField] private float amplitudeX = 5f;       
    [SerializeField] private float speed = 1f;         

    [Header("Дополнительно")]
    [SerializeField] private bool rotateWhileFloating = false;
    [SerializeField] private float rotationAmplitude = 2f; 
    [SerializeField] private bool useUnscaledTime = false; 

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float t = useUnscaledTime ? Time.unscaledTime : Time.time;
        float time = t * speed + timeOffset;

        float offsetY = Mathf.Sin(time) * amplitudeY;
        float offsetX = Mathf.Cos(time * 0.7f) * amplitudeX; 

        transform.localPosition = startPosition + new Vector3(offsetX, offsetY, 0f);

        if (rotateWhileFloating)
        {
            float angle = Mathf.Sin(time * 0.8f) * rotationAmplitude;
            transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
        }
    }
}