using UnityEngine;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    public Transform target;

    public float followHeight = 1.5f;
    public float mouseSensitivity = 2f;
    public float minPitch = -10f;
    public float maxPitch = 45f;

    private float yaw;
    private float pitch = 15f;

    void LateUpdate()
    {
        if (target == null) return;

        // Posisi CameraPivot selalu ikut Player
        transform.position = target.position + Vector3.up * followHeight;

        // Mouse untuk muter kamera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}