using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of player movement
    public float turnSpeed = 100f; // Speed of player turning
    public Transform playerBase;
    public Transform aiFlag;
    public Transform playerFlag;

    private int playerScore = 0;
    private bool hasAiFlag = false;
    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector3 aiFlagInitialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Freeze rotation so we can control rotation manually
    }

    void FixedUpdate()
    {
        // Player movement
        float moveInput = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveInput * moveSpeed;
        rb.AddForce(movement);

        // Player turning (rotation)
        float turnInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void Update()
    {
        // Check if the player picks up the AI flag
        if (Input.GetKeyDown(KeyCode.Space) && !hasAiFlag && Vector3.Distance(transform.position, aiFlag.position) < 1f)
        {
            hasAiFlag = true;
            aiFlag.gameObject.SetActive(false); // Hide the AI flag
        }

        // Check if the player returns to the player base area with the AI flag
        if (hasAiFlag && Vector3.Distance(transform.position, playerBase.position) < 1f)
        {
            playerScore++;
            Debug.Log("Player Score: " + playerScore);

            if (playerScore >= 5)
            {
                // Player wins, restart the game
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                // Reset the position of the AI flag
                aiFlag.position = aiFlagInitialPosition;
                aiFlag.gameObject.SetActive(true);
            }

            // Reset flag status
            hasAiFlag = false;
        }
    }
}
