using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class DogController : MonoBehaviour
{
    public Transform player; // Reference to the XR Rig's camera
    public Vector3 offset = new Vector3(1f, 1.5f, 6f); // Offset from the player's position (to the right)

    public float speed = 2f; // Speed at which the dog follows the player
    private GameObject dog;
    Animator animator;
    float alpha = 0.2f;
    public float movementThreshold = 0.1f;

    public float runDistanceThreshold = 1f; // Distance threshold for running behavior
    private Vector3 previousPlayerPosition;

    public float rotationSpeed = 1f;
    public float slowingDistance = 2f; // Distance at which the dog starts slowing down
    public float arrivalRadius = 0.1f; // Radius within which the dog considers itself arrived

    private Logger logger;

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

        // Initialize the logger
        logger = new Logger("DogBehaviorLog1.csv");
        if (logger == null)
        {
            Debug.LogError("Logger failed to initialize.");
        }
        else
        {
            logger.Log("Game 1 Dog Behavior started");
        }
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

        // Log player position
        logger.Log($"PlayerPosition: {player.position}, DogPosition: {transform.position}, Time: {Time.time}");

        // Set the animation state based on the distance difference
        if (distanceDifference > 0.01f)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Roaming", false); // Run animation
            logger.Log($"DogRunning: true, DistanceDifference: {distanceDifference}, Time: {Time.time}");
        }
        else
        {
            animator.SetBool("Run", false); // Sit animation
            animator.SetBool("Roaming", false);
            logger.Log($"DogRunning: false, DistanceDifference: {distanceDifference}, Time: {Time.time}");
        }

        // Calculate the target position for the dog (with an offset if needed)
        Vector3 targetPosition = player.position + player.forward * offset.z + player.right * offset.x;
        targetPosition.y = transform.position.y; // Preserve the y-axis

        // Calculate the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Log the target position and distance
        logger.Log($"TargetPosition: {targetPosition}, DistanceToTarget: {distanceToTarget}, Time: {Time.time}");

        // Check if the dog has arrived at the target
        if (distanceToTarget <= arrivalRadius)
        {
            logger.Log($"DogArrived: true, ArrivalRadius: {arrivalRadius}, Time: {Time.time}");
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

        // Rotate the dog to face the direction it's moving
        Vector3 velocity = GetComponent<Rigidbody>().velocity;

        // Only rotate if velocity is significant
        if (velocity.magnitude > 0.1f) // Adjust the threshold as needed
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Adjust the rotation speed factor

            // Set the run and rotate animations
            animator.SetBool("Run", true);
            animator.SetBool("Roaming", true);
            logger.Log($"DogRotating: true, Velocity: {velocity.magnitude}, Time: {Time.time}");
        }
        else
        {
            // Set the idle animation
            animator.SetBool("Run", false);
            animator.SetBool("Roaming", false);
            logger.Log($"DogIdle: true, Velocity: {velocity.magnitude}, Time: {Time.time}");
        }
    }
}
