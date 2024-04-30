using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{

    public Transform player; // Reference to the XR Rig's camera

    public float speed = 3f; // Speed at which the dog follows the player
    public float rotationSpeed = 5f; // Speed at which the dog rotates towards the player

    private void Update()
    {
        // Check if the player reference is set

         Vector3 targetPosition = player.position;

        Debug.Log("player position"+targetPosition);
        if (player == null)
        {
            Debug.LogError("Player reference is not set in DogController!");
            return;
        }

        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Get the position of the XR Rig's camera
        Vector3 targetPosition = player.position;

        Debug.Log("player position"+targetPosition);
        targetPosition.y = transform.position.y; // Ignore changes in the vertical axis

        // Calculate direction to move towards the player
        Vector3 direction = targetPosition - transform.position;

        // Check if direction vector is non-zero before attempting rotation
        if (direction != Vector3.zero)
        {
            // Rotate the dog towards the player
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move the dog towards the player
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
