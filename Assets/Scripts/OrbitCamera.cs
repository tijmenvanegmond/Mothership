using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class OrbitCamera : MonoBehaviour
{
	public GameObject Target;
	public float MaxDistance = 100f;
	public float Distance = 10f;
	public float OrbitSpeed = 160f;
	public float RollSpeed = 80f;
	//public Vector3 StartRotation = new Vector3(45f, 0, 0);
	private Vector3 mouseRotation = Vector3.zero;
	private Quaternion totalRotation = Quaternion.identity;
	private Quaternion addedRollRotation = Quaternion.identity;
	private bool buidMode = false;

	void Update()
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		Vector3 movementVector = new Vector3(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0);
		Vector3 rotationInputVector =
			new Vector3(Input.GetAxis("ShipPitch"), Input.GetAxis("ShipYaw"), Input.GetAxis("ShipRoll"));

		if (Input.GetButtonUp("Build Mode")) buidMode = !buidMode;

		if (buidMode)
		{
			totalRotation *= Quaternion.Euler(movementVector * OrbitSpeed * Time.deltaTime);
		}
		else
		{
			//Only orbit in the biggest axis
			if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
			{
				totalRotation *= Quaternion.Euler(new Vector3(0, mouseDelta.x * OrbitSpeed * Time.deltaTime, 0));
			}
			else
			{
				totalRotation *= Quaternion.Euler(new Vector3(mouseDelta.y * -OrbitSpeed * Time.deltaTime, 0, 0));
			}
		}

		addedRollRotation = Quaternion.Euler(new Vector3(0, 0, rotationInputVector.z * -RollSpeed * Time.deltaTime));
		totalRotation *= addedRollRotation;

		Distance -= Input.mouseScrollDelta.y * Time.deltaTime * 50;
		//make surte its not too far or close
		Distance = Mathf.Clamp(Distance, GetTargetBoundRadius(), MaxDistance);

		PlaceCameraWithRay();
	}

	float GetTargetBoundRadius()
	{
		Collider[] targetColliders = Target.GetComponentsInChildren<Collider>();
		Bounds targetBounds = new Bounds(Target.transform.position, Vector3.zero);
		foreach (Collider nextCollider in targetColliders)
			targetBounds.Encapsulate(nextCollider.bounds);

		//distance from target pos to center of bounding box + the extend of the bounding box
		return (targetBounds.center - Target.transform.position).magnitude + targetBounds.extents.magnitude;
	}


	void PlaceCameraWithRay()
	{
		Vector3 cameraDirection = -(totalRotation * Vector3.forward);
		Ray ray = new Ray(Target.transform.position + (cameraDirection * Distance), -cameraDirection);

		try
		{
			ray = GetPointOfVisibilty(ray);
		}
		catch
		{
			Debug.Log("camera can't see the ship");
		}
		//TODO: camera still ends up in geometry
		transform.position = ray.origin + (ray.direction * .5f);

		transform.rotation = totalRotation;
	}
	/// <summary>
	/// casts a ray and continues to cast it trought object until the target can been seen
	/// </summary>
	/// <param name="ray">a ray aimed at the target</param>
	/// <returns>a ray that will hit the target</returns>
	/// <exception cref="Exception">can't find the bloody target</exception>
	Ray GetPointOfVisibilty(Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Distance))
		{
			if (hit.rigidbody != null && hit.rigidbody.gameObject == Target)
			{
				return ray;
			}
			else
			{
				return GetPointOfVisibilty(new Ray(hit.point, ray.direction));
			}
		}
		else throw new Exception("ray not aimed at target or target does not match ");
	}
}