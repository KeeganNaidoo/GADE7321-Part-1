using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow (usually the player)
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the target position
    public float smoothSpeed = 10f; // Smoothness of the camera movement

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
