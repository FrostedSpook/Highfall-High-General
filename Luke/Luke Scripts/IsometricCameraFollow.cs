using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    public Transform target;            // Player or object to follow
    public Vector3 offset = new Vector3(10f, 10f, -10f); // Isometric offset
    public float smoothSpeed = 5f;      // Camera smoothing speed

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target); // Optional: make the camera always look at the player
    }
}