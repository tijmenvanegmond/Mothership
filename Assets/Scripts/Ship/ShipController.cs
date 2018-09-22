using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class ShipController : MonoBehaviour
{
	public Ship Ship;
	public GameObject Camera;

	public bool RotateWithCamera = true;
	public float RotationSpeed = 1f;
	public Vector3 RotationMultiplier = Vector3.one;
	public bool IsDampening = false;
	private Vector3 movementInputVector;
	private Vector3 rotationInputVector;
	private Rigidbody rBody;
	private float drag;
	private float angularDrag;

	void Start()
	{
		Ship = GetComponent<Ship>() as Ship;
		rBody = GetComponent<Rigidbody>() as Rigidbody;
		//store defaults
		drag = rBody.drag;
		angularDrag = rBody.angularDrag;
	}

	void Update()
	{
		movementInputVector = new Vector3(Input.GetAxis("ShipXAxis"), Input.GetAxis("ShipYAxis"), Input.GetAxis("ShipThrottle"));
		rotationInputVector = new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

		if (Input.GetButtonUp("Dampening")) IsDampening = !IsDampening;
		if (Input.GetMouseButtonUp(2)) RotateWithCamera = !RotateWithCamera;
	}

	void FixedUpdate()
	{
		if (RotateWithCamera)
		{
			//Rotate x&y-axis
			RotateAxisToAlign(transform.forward, Camera.transform.forward);
			//Added this to add z-axis rotation but that also means x-axis is doubly applied (not terrible)
			RotateAxisToAlign(transform.up, Camera.transform.up);
		}

		//Dampening
		//TODO: less escalating / less figthing
		if (IsDampening)
		{
			var v = rBody.velocity;
			var angularV = rBody.angularVelocity;
			foreach (var truster in Ship.Trusters)
			{
				//dampen momentum
				var dot = Vector3.Dot(v.normalized, truster.GetForceDirection().normalized);
				if (dot < 0)
				{
					Vector3 force = truster.GetForceVector();
					Vector3 postion = truster.GetForcePostion();
					truster.FireTuster(Mathf.Min(.5f, -dot));
				}

				//dampen angular momentum
				dot = Vector3.Dot(angularV.normalized, truster.GetExpectedAngularAcceleration().normalized);
				if (dot < 0)
				{
					Vector3 force = truster.GetForceVector();
					Vector3 postion = truster.GetForcePostion();
					truster.FireTuster(Mathf.Min(.2f, -dot));
				}

			}
		}

		//Movement - curently based on camera axis
		//TODO: stop lowering max speed when goind multiple axis
		var gMoveInputVector = Ship.transform.TransformDirection(movementInputVector);
		foreach (var truster in Ship.Trusters)
		{
			var dot = Vector3.Dot(gMoveInputVector, truster.GetForceDirection());
			if (dot > 0)
			{
				Vector3 force = truster.GetForceVector();
				Vector3 postion = truster.GetForcePostion();
				truster.FireTuster(dot);
			}
		}
	}

	/// <summary>
	/// Rotates the ship usting its trusters so both provided axises are aligned
	/// </summary>
	/// <param name="shipAxis"> Axis of the ship in worldspace</param>
	/// <param name="targetAxis"> Axis to align shipAxis with in worldspace</param>
	void RotateAxisToAlign(Vector3 shipAxis, Vector3 targetAxis)
	{
		//TODO: less overshooting
		var axisCross = Vector3.Cross(shipAxis, targetAxis);
		var dotDiff = Vector3.Dot(shipAxis, targetAxis);
		dotDiff = .5f - (dotDiff * .5f); //make it go form 0 to 10
		dotDiff += .2f;

		foreach (var truster in Ship.Trusters)
		{
			var dot = Vector3.Dot(axisCross.normalized, truster.GetTorqueAxis().normalized);
			if (dot <= 0) //torque axis not alligned
				continue;

			Vector3 force = truster.GetForceVector();
			Vector3 postion = truster.GetForcePostion();
			truster.FireTuster(dot * dotDiff);
		}
	}

	//An atempt at a new rotation syatem
	void SmartAxisRotation()
	{
		var localAngularV = transform.InverseTransformDirection(rBody.angularVelocity);
		//Rotation TODO: fix over compensation
		var cameraAngle = Camera.transform.rotation.eulerAngles;
		var shipAngle = transform.rotation.eulerAngles;
		var aD = new Vector3(Mathf.DeltaAngle(cameraAngle.x, shipAngle.x), Mathf.DeltaAngle(cameraAngle.y, shipAngle.y), Mathf.DeltaAngle(cameraAngle.z, shipAngle.z));
		var radianDelta = aD * Mathf.Deg2Rad;

		float speed = localAngularV.z;
		float distance = radianDelta.z;
		float maxDecel = 0;

		foreach (var truster in Ship.Trusters)
		{
			var localAA = transform.InverseTransformDirection(truster.GetExpectedAngularAcceleration());
			var z = localAA.z;
			if ((distance > 0 && z < 0) || (distance < 0 && z > 0))
			{
				maxDecel += z;
			}
		}

		if (distance / speed >= speed / maxDecel)
		{
			foreach (var truster in Ship.Trusters)
			{
				var localAA = transform.InverseTransformDirection(truster.GetExpectedAngularAcceleration());
				var z = localAA.z;
				if ((distance > 0 && z < 0) || (distance < 0 && z > 0))
				{
					truster.FireTuster(1);
				}
			}
		}
		else
		{
			foreach (var truster in Ship.Trusters)
			{
				var localAA = transform.InverseTransformDirection(truster.GetExpectedAngularAcceleration());
				var z = localAA.z;
				if (distance > 0 && z > 0)
				{
					truster.FireTuster(1);
				}
				if (distance < 0 && z < 0)
				{
					truster.FireTuster(1);
				}

			}
		}
	}

}
