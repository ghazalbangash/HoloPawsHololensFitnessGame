using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DogController : MonoBehaviour
{

    public Transform player; // Reference to the XR Rig's camera
    public Vector3 offset = new Vector3(1f, 1.5f, 6f); // Offset from the player's position (to the right)

    public float speed = 5f; // Speed at which the dog follows the player
    private GameObject dog ;
    Animator animator;
    float alpha = 0.2f;
    public float movementThreshold = 0.1f;

    public float runDistanceThreshold = 1f; // Distance threshold for running behavior
    private Vector3 previousPlayerPosition;
    private float lookDuration = 0f; // Duration in seconds the player has been looking in a direction
    private const float lookDurationThreshold = 6f;



    public float slowingDistance = 2f; // Distance at which the dog starts slowing down
    public float arrivalRadius = 0.1f; // Radius within which the dog considers itself arrived


    private void Awake()
    {
        // Find the player GameObject by tag
        dog = GameObject.FindWithTag("Dog");
        animator = GetComponent<Animator>();



        if (player == null)
        {
            Debug.LogError("Player GameObject with tag '" + dog + "' not found!");
        }
        previousPlayerPosition = player.position;
        // Vector3 targetPosition1 = player.position + player.right * offset.x + player.forward * offset.z;

        // // Move the dog towards the target position
        // dog.transform.position = Vector3.MoveTowards(dog.transform.position, targetPosition1, speed );
               
    }


    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in DogController!");
            return;
        }


        float distanceDifference = Vector3.Distance(player.position, previousPlayerPosition);

        // Update previous player position for the next frame
        previousPlayerPosition = player.position;

        // Set the animation state based on the distance difference
        if (distanceDifference > 0.01f) 
        {

            animator.SetBool("Run", true); // Run animation
        }
        else
        {

            animator.SetBool("Run", false); // Sit animation
        }

        // Calculate the target position for the dog (with an offset if needed)
        //Vector3 targetPosition = player.position;
        Vector3 targetPosition = player.position + player.forward * offset.z + player.right * offset.x;

        // Calculate the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Check if the dog has arrived at the target
        if (distanceToTarget <= arrivalRadius)
        {
            // Dog has arrived, no need to move
            return;
        }

        // Calculate the desired velocity using arrive behavior
        float desiredSpeed = Mathf.Min(speed, speed * (distanceToTarget / slowingDistance));
        Vector3 desiredVelocity = directionToTarget.normalized * desiredSpeed;

        // Calculate steering force
        Vector3 steeringForce = desiredVelocity - GetComponent<Rigidbody>().velocity;

        // Apply steering force to adjust the dog's velocity
        GetComponent<Rigidbody>().velocity += steeringForce * Time.deltaTime;

        // Update dog's position based on its velocity
        transform.position += GetComponent<Rigidbody>().velocity * Time.deltaTime;
    }



}
