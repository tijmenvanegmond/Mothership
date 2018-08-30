using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class ShipController : MonoBehaviour
{
	public Ship ship;
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
		ship = GetComponent<Ship>() as Ship;
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

		if (IsDampening)
		{
			rBody.angularDrag = .8f;
			rBody.drag = .2f;
		}
		else
		{
			rBody.drag = drag;
			rBody.angularDrag = angularDrag;
		}
	}

	void FixedUpdate()
	{
		if (RotateWithCamera)
		{
			rBody.MoveRotation(Quaternion.Slerp(transform.rotation, Camera.transform.rotation,
				Time.deltaTime * RotationSpeed));
		}

		var gMoveInputVector = transform.TransformDirection(movementInputVector);
		foreach (var truster in ship.Trusters)
		{
			var dot = Vector3.Dot(gMoveInputVector, truster.GetForceDirection());
			if(dot > 0)
			{
				Vector3 force = truster.GetForceVector();
				Vector3 postion = truster.GetForcePostion();
				truster.FireAt(dot);
			}
		}
	}
}
