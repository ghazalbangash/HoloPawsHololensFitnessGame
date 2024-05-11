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
        Vector3 targetPosition1 = player.position + player.right * offset.x + player.forward * offset.z;

        // Move the dog towards the target position
        dog.transform.position = Vector3.MoveTowards(dog.transform.position, targetPosition1, speed );
               
    }


    private void Update()
    {

        // Calculate the distance between the player and the dog
        // Calculate the difference between current and previous player position
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


    //    dog.transform.position = new Vector3(player.transform.position.x + -offset.x, player.transform.position.y - + offset.y,player.transform.position.z + offset.z);

    //     //Calculate the dog's target position based on the player's position and offset
    //     Vector3 targetPosition1 = player.position + player.right * offset.x + player.forward * offset.z;

    //     // Move the dog towards the target position
    //     dog.transform.position = Vector3.MoveTowards(dog.transform.position, targetPosition1, speed * Time.deltaTime);

    //     //Calculate the direction from the dog to the player
    //     Vector3 directionToPlayer =  dog.transform.position- player.position ;

    //     //Make the dog face the same direction as the player
    //     if (directionToPlayer != Vector3.zero)
    //     {
    //         Quaternion targetRotation = Quaternion.LookRotation(-(directionToPlayer + Vector3.up * 0.2f));
    //         dog.transform.rotation = Quaternion.RotateTowards(dog.transform.rotation, targetRotation, speed * Time.deltaTime);
        
    //     }


        Vector3 directionToDog = dog.transform.position - player.position;

        if (Vector3.Dot(player.forward, directionToDog) > 0.01f)
        {

            lookDuration += Time.deltaTime;
            Debug.Log("not looking dog");
        }
        else
        {
            lookDuration = 0f;
            Debug.Log("looking");
        }

        // Update dog's target position if player has looked in a direction for more than the threshold

        if (lookDuration >= lookDurationThreshold)
        {


            Vector3 targetPosition = player.position + player.forward * offset.z + player.right * offset.x;
            // Move the dog towards the target position
            dog.transform.position = Vector3.MoveTowards(dog.transform.position, targetPosition, speed * Time.deltaTime);


            // Make the dog face the same direction as the player
            Vector3 directionToPlayer =  dog.transform.position- player.position;



            if (directionToPlayer != Vector3.zero)
            {
                Debug.Log("whats happening here "+ directionToPlayer);
                Quaternion targetRotation = Quaternion.LookRotation(-(directionToPlayer + Vector3.up * 0.2f));
                dog.transform.rotation = Quaternion.RotateTowards(dog.transform.rotation, targetRotation, speed * Time.deltaTime);
            }

            



            //lookDuration = 0f;
        }




    }

}
