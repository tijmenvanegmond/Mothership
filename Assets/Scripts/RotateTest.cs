using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RotateTest : MonoBehaviour
{

	public GameObject Target;

	private Vector3 movementInputVector;
	private bool isActive;

	private Rigidbody rBody;
	private float drag;
	private float angularDrag;
	[SerializeField]
	private float FORCE_MULTIPLIER = 10f;
	private float MaxAngularSpeed = 120f;
	private float MaxAngularAcceleration = 2f;
	private const float RadToDeg = 57.2957795131f;

	void Start()
	{
		rBody = GetComponent<Rigidbody>() as Rigidbody;
		//store defaults
		drag = rBody.drag;
		angularDrag = rBody.angularDrag;
	}

	void Update()
	{
		if (Input.GetButtonUp("Dampening")) isActive = !isActive;
		movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
	}

	void FixedUpdate()
	{
		rBody.AddRelativeTorque(movementInputVector * FORCE_MULTIPLIER);

		if (isActive)
		{
			//RotateXToTarget();
			RotateYToTarget();
			//RotateZToTarget();
		}
	}

	void RotateXToTarget()
	{
		//per axis rotation
		var axis = transform.localEulerAngles.x;
		var targetAxis = Target.transform.localEulerAngles.x;
		var deltaAngle = Mathf.DeltaAngle(axis, targetAxis); //Deg
		var velocity = rBody.transform.InverseTransformVector(rBody.angularVelocity).x * RadToDeg; //DegPerSec
		var breakDistance = (velocity * velocity) / (2f * MaxAngularAcceleration); //in degrees
		Debug.Log("DeltaAngle:"+deltaAngle);
		Debug.Log("breakDistance: " + breakDistance);

		if (breakDistance < Mathf.Abs(deltaAngle)+10f)
		{
			Debug.Log("Accelerating");
			//Accelerate
			var goalDirection = deltaAngle < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.right * MaxAngularAcceleration * goalDirection, ForceMode.Acceleration);
		}
		else if (breakDistance > 0.01f)
		{
			Debug.Log("Deccelerating");
			//Deccelerate
			var velocityDirection = velocity < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.right * MaxAngularAcceleration * -velocityDirection, ForceMode.Acceleration);
		}
	}

	void RotateYToTarget()
	{
		//per axis rotation
		var axis = transform.localEulerAngles.y;
		var targetAxis = Target.transform.localEulerAngles.y;
		var deltaAngle = Mathf.DeltaAngle(axis, targetAxis); //Deg
		var velocity = rBody.transform.InverseTransformVector(rBody.angularVelocity).y * RadToDeg; //DegPerSec
		var breakDistance = (velocity * velocity) / (2f * MaxAngularAcceleration); //in degrees
		Debug.Log("DeltaAngle:" + deltaAngle);
		Debug.Log("breakDistance: " + breakDistance);

		if (breakDistance < Mathf.Abs(deltaAngle) + 10f)
		{
			Debug.Log("Accelerating");
			//Accelerate
			var goalDirection = deltaAngle < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.up * MaxAngularAcceleration * goalDirection, ForceMode.Acceleration);
		}
		else if (breakDistance > 0.01f)
		{
			Debug.Log("Deccelerating");
			//Deccelerate
			var velocityDirection = velocity < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.up * MaxAngularAcceleration * -velocityDirection, ForceMode.Acceleration);
		}
	}

	void RotateZToTarget()
	{
		//per axis rotation
		var axis = transform.localEulerAngles.z;
		var targetAxis = Target.transform.localEulerAngles.z;
		var deltaAngle = Mathf.DeltaAngle(axis, targetAxis); //Deg
		var velocity = rBody.transform.InverseTransformVector(rBody.angularVelocity).z * RadToDeg; //DegPerSec
		var breakDistance = (velocity * velocity) / (2f * MaxAngularAcceleration); //in degrees
		Debug.Log("DeltaAngle:" + deltaAngle);
		Debug.Log("breakDistance: " + breakDistance);

		if (breakDistance < Mathf.Abs(deltaAngle) + 10f)
		{
			Debug.Log("Accelerating");
			//Accelerate
			var goalDirection = deltaAngle < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.up * MaxAngularAcceleration * goalDirection, ForceMode.Acceleration);
		}
		else if (breakDistance > 0.01f)
		{
			Debug.Log("Deccelerating");
			//Deccelerate
			var velocityDirection = velocity < 0f ? -1f : 1f;
			rBody.AddRelativeTorque(Vector3.up * MaxAngularAcceleration * -velocityDirection, ForceMode.Acceleration);
		}
	}

}
