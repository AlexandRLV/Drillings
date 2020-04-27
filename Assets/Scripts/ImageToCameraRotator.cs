using UnityEngine;

public class ImageToCameraRotator : MonoBehaviour
{
    private Transform cameraTransform;

    private void OnEnable()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.forward = transform.position - cameraTransform.position;
    }
}
