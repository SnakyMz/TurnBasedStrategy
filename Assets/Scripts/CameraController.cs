using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineFollow cinemachineFollow;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 50f;
    [SerializeField] float zoomMaxOffsetY = 7f;
    [SerializeField] float zoomMinOffsetY = 1f;

    Vector2 moveInput;
    float rotateInput;
    float zoomInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        transform.position += inputDirection * moveSpeed * Time.deltaTime;

        Vector3 rotateDirection = new Vector3(0, rotateInput, 0);
        transform.eulerAngles += rotateDirection * rotateSpeed * Time.deltaTime;

        cinemachineFollow.FollowOffset.y += zoomInput;
        cinemachineFollow.FollowOffset.y = Mathf.Clamp(cinemachineFollow.FollowOffset.y, zoomMinOffsetY, zoomMaxOffsetY);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<float>();
    }
}
