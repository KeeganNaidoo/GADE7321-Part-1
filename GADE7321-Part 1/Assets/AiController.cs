using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    Capture,
    Chase,
    ReturnToBase
}

public class AIController : MonoBehaviour
{
    public AIState currentState = AIState.Capture;
    
    public Transform player;
    public Transform playerFlag;
    public Transform aiFlag;
    public Transform playerBase;
    public Transform aiBase;

    public float moveSpeed = 5f; // Speed at which the AI moves
    private bool hasPlayerFlag = false;
    private bool hasAiFlag = false;
    private int aiScore = 0;
    private Vector3 aiFlagInitialPosition;
    private Vector3 playerFlagInitialPosition;

    void Start()
    {
        // Assume the AI agent starts with no flag
        hasAiFlag = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Capture:
                Capture();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.ReturnToBase:
                ReturnToBase();
                break;
        }
    }

    void Capture()
    {
        // If the player's flag is at the AI's position, capture it
        if (Vector3.Distance(transform.position, playerFlag.position) < 0.1f)
        {
            hasPlayerFlag = true;
            playerFlag.gameObject.SetActive(false); // Hide the player's flag
        }
        // Otherwise, move towards the player's flag's position
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerFlag.position, Time.deltaTime * moveSpeed);
        }
    }

    void Chase()
    {
        // Move towards the player's position
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
    }

    void ReturnToBase()
    {
        // Move towards the AI's base 
        transform.position = Vector3.MoveTowards(transform.position, aiBase.position, Time.deltaTime * moveSpeed);
        Debug.Log("AI ENEMY IS RETURNING TO BASE!");
    }

    void OnTriggerEnter(Collider other)
    {
        // If the AI collides with the player and has the player's flag, stop and drop the flag
        if (other.CompareTag("Player") && hasPlayerFlag)
        {
            hasPlayerFlag = false;
            playerFlag.position = playerFlagInitialPosition;
            playerFlag.gameObject.SetActive(true);
        }
        // If the AI collides with the player and has the AI flag, reset the flag position and increase AI score
        else if (other.CompareTag("Player") && hasAiFlag)
        {
            hasAiFlag = false;
            aiFlag.position = aiFlagInitialPosition;
            aiFlag.gameObject.SetActive(true);
            aiScore++;
            Debug.Log("AI Score: " + aiScore);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the AI is no longer colliding with the player, resume capturing or returning to base
        if (other.CompareTag("Player") && hasPlayerFlag)
        {
            currentState = AIState.Capture; // Or AIState.ReturnToBase, depending on your logic
        }
    }
}
