using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTest : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 controlVector;
    public Transform target;

    public float rotationSpeed = 1.0f;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RBAddForce(transform.forward * speed);

        Vector3 targetDirection = target.position - transform.position;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void RBAddForce(Vector3 dir)
    {
        rb.AddForce(dir, ForceMode.Acceleration);
    }
}
