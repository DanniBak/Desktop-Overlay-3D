using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float cohesionStrength = 1.0f;
    public float separationStrength = 1.0f;
    public float alignmentStrength = 1.0f;
    public float randomnessStrength = 1.0f; // Strength of random movement
    public float neighborRadius = 2.0f;
    public float maxSpeed = 5.0f;
    public Transform target; // Target object for the boids to swarm around


    private void Start()
    {
        target = GameObject.Find("Target").transform;
    }

    private void FixedUpdate()
    {
        Vector3 cohesionVector = Vector3.zero;
        Vector3 separationVector = Vector3.zero;
        Vector3 alignmentVector = Vector3.zero;
        Vector3 randomnessVector = Random.insideUnitSphere * randomnessStrength; // Random vector

        // Perform a spherecast to find neighbors
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, neighborRadius, transform.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Bee"))
            {
                Boid otherBoid = hit.collider.GetComponent<Boid>();
                if (otherBoid != null && otherBoid != this)
                {
                    float distance = Vector3.Distance(transform.position, otherBoid.transform.position);

                    // Cohesion
                    cohesionVector += otherBoid.transform.position;

                    // Separation
                    separationVector += (transform.position - otherBoid.transform.position) / distance;

                    // Alignment
                    alignmentVector += otherBoid.transform.forward;
                }
            }
        }

        // Calculate average values for cohesion, separation, and alignment
        cohesionVector /= hits.Length;
        separationVector /= hits.Length;
        alignmentVector /= hits.Length;

        // Calculate the desired direction with added randomness
        Vector3 desiredDirection = (cohesionVector * cohesionStrength + separationVector * separationStrength + alignmentVector * alignmentStrength + randomnessVector).normalized;

        // If a target object is specified, add attraction towards the target
        if (target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            desiredDirection = (desiredDirection + targetDirection).normalized;
        }

        // Update the boid's rotation to face the desired direction
        transform.forward = Vector3.Slerp(transform.forward, desiredDirection, Time.deltaTime);

        // Move the boid forward
        transform.position += transform.forward * maxSpeed * Time.deltaTime;

    }
}
