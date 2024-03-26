using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distance = 5f; // Distance from the player
    public float height = 2f; // Height of the camera above the player
    public float rotationSpeed = 100f; // Speed of rotation

    private Vector3 offset; // Offset from the player's position

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set in CameraController!");
            return;
        }

        offset = transform.position - player.position; // Calculate the initial offset
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Rotate the player based on input
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        player.Rotate(Vector3.up * rotationInput);

        // Calculate the desired position of the camera
        Quaternion rotation = Quaternion.Euler(0, player.eulerAngles.y, 0); // Apply player's rotation
        Vector3 targetPosition = player.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

        // Make the camera look at the player
        transform.LookAt(player);
    }
}
