using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    private GameObject carriedObject; // The object currently being carried
    private Vector3 objectOffset = new Vector3(0f, 1f, 0f); // Offset for the object above the player

    private GameObject player; // Reference to the player GameObject
    private GameObject aiAgent; // Reference to the AI agent GameObject

    private Vector3 playerInitialPosition; // Initial position of the player
    private Vector3 aiAgentInitialPosition; // Initial position of the AI agent

    private int score = 0; // Player or AI agent score

    void Start()
    {
        // Find and store references to the player and AI agent GameObjects
        player = GameObject.FindGameObjectWithTag("Player");
        aiAgent = GameObject.FindGameObjectWithTag("AI");

        // Store initial positions of the player and AI agent
        playerInitialPosition = player.transform.position;
        aiAgentInitialPosition = aiAgent.transform.position;
    }

    void Update()
    {
        // Check for input to pick up or drop the object
        if (Input.GetKeyDown(KeyCode.E) && carriedObject == null)
        {
            PickUpObject();
        }
        else if (Input.GetKeyDown(KeyCode.E) && carriedObject != null)
        {
            DropObject();
        }
    }

    void PickUpObject()
    {
        // Raycast to check for objects within range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
        {
            // Check if the object is interactable
            if (hit.collider.CompareTag("Interactable"))
            {
                // Pick up the object
                carriedObject = hit.collider.gameObject;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                carriedObject.transform.SetParent(transform); // Attach to player
                carriedObject.transform.localPosition = objectOffset; // Set offset position
            }
            else if (hit.collider.CompareTag("PlayerBaseArea") && carriedObject != null)
            {
                // Pass through the waypoint and earn a point only if carrying the object
                EarnPoint();
                
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is carrying the object and passes through the waypoint
        if (other.CompareTag("PlayerBaseArea") && carriedObject != null)
        {
            EarnPoint();
        }
    }

    void DropObject()
    {
        // Drop the object
        carriedObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
        carriedObject.transform.SetParent(null); // Detach from player
        carriedObject = null; // Reset carried object
    }

    void EarnPoint()
    {
        score++;
        Debug.Log("Point earned! Score: " + score);

        // Check if one point is earned
        if (score >= 1)
        {
            // Reset object position
            ResetObjectPosition();

            // Reset player and AI agent positions
            ResetPlayerPosition();
            ResetAIAgentPosition();
        }
    }

    void ResetObjectPosition()
    {
        // Reset object position to initial position
        carriedObject.transform.position = Vector3.zero; 
    }

    void ResetPlayerPosition()
    {
        // Reset player position to initial position
        player.transform.position = playerInitialPosition;
    }

    void ResetAIAgentPosition()
    {
        // Reset AI agent position to initial position
        aiAgent.transform.position = aiAgentInitialPosition;
    }
}
