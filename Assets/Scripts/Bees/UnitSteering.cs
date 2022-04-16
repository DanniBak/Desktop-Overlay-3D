using UnityEngine;
using System.Collections;

public class UnitSteering : MonoBehaviour {

    public Vector3 moveDir;
    public GameObject target;
    public float moveSpeed;
	Rigidbody rb;

	public float rotationSpeed = 1.0f;

	//Steering variables
	public float pursuitWeight = 1;
	public float seperationWeight = 1;
	public float cohesionWeight = 1;
	public float alignmentWeight = 1;
	public float avoidanceWeight = 1;
	public float slowing_distance = 0;

	//Debug variables
	public int numberOfNearbyCreatures = 0;
    public float overallSpeed;

    private void Start()
    {
        target = GameObject.Find("Target");
		rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {

		//If a target exists, set it's position as pursuit
		if (target != null){
	    moveDir = (target.transform.position - transform.position).normalized * pursuitWeight;
	    	}



		Vector3 target_offset = target.transform.position - transform.position;
	float offsetLength = target_offset.sqrMagnitude;
	float ramped_speed = moveSpeed * (offsetLength / slowing_distance);
	float clipped_speed = Mathf.Min(ramped_speed, moveSpeed);
	Vector3 desired_velocity = (clipped_speed / offsetLength) * target_offset;
	moveDir += desired_velocity;


	Vector3 averageNeighbourPosition = Vector3.zero;
	Vector3 averageNeighbourDirection = Vector3.zero;
	numberOfNearbyCreatures = 0;

	//Check for neighbours in r=5 sphere centered on self
	Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 5);
	
	for(int i = 0; i < nearbyObjects.Length; i++){
	    
	    if(nearbyObjects[i].gameObject.tag == "Bee" && nearbyObjects[i] != GetComponent<Collider>()) {
	    
		//Calculate the average position and what direction they're moving in of all neighbours
		averageNeighbourPosition += nearbyObjects[i].transform.position;
		averageNeighbourDirection += nearbyObjects[i].transform.forward;
		numberOfNearbyCreatures++;
		
		Vector3 offset = nearbyObjects[i].transform.position - transform.position;
		moveDir += (offset / -offset.sqrMagnitude) * seperationWeight;	    
	    }
	}

	if(numberOfNearbyCreatures > 0){
	    averageNeighbourPosition /= numberOfNearbyCreatures;
	    averageNeighbourDirection /= numberOfNearbyCreatures;

	    moveDir += (averageNeighbourPosition - transform.position).normalized * cohesionWeight;
	    moveDir += averageNeighbourDirection.normalized * alignmentWeight;
	    
	}
	

	moveDir = moveDir.normalized *  moveSpeed;

		rb.AddForce(moveDir, ForceMode.Acceleration);

		
	if(moveDir != Vector3.zero){
		Vector3 newDirection = Vector3.RotateTowards(transform.forward, target_offset, rotationSpeed, 0.0f);
		Debug.DrawRay(transform.position, newDirection, Color.red);
		transform.rotation = Quaternion.LookRotation(newDirection);
	    transform.forward = new Vector3(moveDir.x , moveDir.y, moveDir.z);
	}
	}

}
