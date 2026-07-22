using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(mainCamera);
        transform.Rotate(0, 180, 0);
    }
}
