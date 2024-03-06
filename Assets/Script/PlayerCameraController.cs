using UnityEngine;
using Unity.Netcode;

public class PlayerCameraController : NetworkBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            // Lock the cursor to the center of the screen and make it invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation for looking up and down
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player for looking left and right
        transform.Rotate(Vector3.up * mouseX);
    }
}
