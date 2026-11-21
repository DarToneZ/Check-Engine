using UnityEngine;

public class ProgressUpdate : MonoBehaviour
{
    [SerializeField] int newProgress;
    void Start()
    {
        PlayerPrefs.SetInt("level", newProgress);   
    }
}
