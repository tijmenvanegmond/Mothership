using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truster : Node {

	public float Trust;
	public Transform[] trusterTransforms;
	private Ship ship;
	//DEBUG
	private Material mat;

	/// <summary>
	/// Axis this truster will provide torque in worldspace
	/// </summary>
	/// <returns></returns>
	public Vector3 GetTorqueAxis()
	{
		var shipCenter = ship.rBody.centerOfMass;
		var pos = ship.transform.InverseTransformPoint(GetForcePostion());
		var vecToCenter = shipCenter - pos;
		var force = ship.transform.InverseTransformDirection(GetForceVector());
		var cross = Vector3.Cross(force, vecToCenter);
		return ship.transform.TransformDirection(cross);
	}

	//COPIED for angular acceleration caluclation
	void ToDeltaTorque(ref Vector3 torque, ForceMode forceMode)
	{
		bool continuous = forceMode == ForceMode.VelocityChange || forceMode == ForceMode.Acceleration;
		bool useMass = forceMode == ForceMode.Force || forceMode == ForceMode.Impulse;

		if (continuous) torque *= Time.fixedDeltaTime;
		if (useMass) ApplyInertiaTensor(ref torque);
	}

	void ApplyInertiaTensor(ref Vector3 v)
	{
		v = ship.rBody.rotation * Div(Quaternion.Inverse(ship.rBody.rotation) * v, ship.rBody.inertiaTensor);
	}

	static Vector3 Div(Vector3 v, Vector3 v2)
	{
		return new Vector3(v.x / v2.x, v.y / v2.y, v.z / v2.z);
	}
	//

	public Vector3 GetExpectedAngularAcceleration()
	{
		var t = GetTorqueAxis();
		ToDeltaTorque(ref t,ForceMode.Impulse);
		return t;
	}

	/// <summary>
	/// Normalized worldspace vector of the direction this truster delivers trust
	/// </summary>
	/// <returns></returns>
	public Vector3 GetForceDirection()
	{
		return transform.up * -1 * transform.localScale.y;
	}

	/// <summary>
	/// Worldspace vector of the force this truster delivers at full trust
	/// </summary>
	/// <returns></returns>
	public Vector3 GetForceVector()
	{
		return transform.up * -Trust * transform.localScale.y;
	}

	/// <summary>
	/// Get the world position from where the trust originates
	/// </summary>
	/// <returns></returns>
	public Vector3 GetForcePostion()
	{
		return transform.position + (transform.up * .5f * transform.localScale.y);
	}

	private void Start()
	{
		ship = transform.parent.GetComponent<Ship>() as Ship;
		///DEBUG
		mat = transform.GetChild(2).GetComponent<Renderer>().material;
	}

	//TODO: make it so they can only 100% per update
	public void FireTuster(float multiplier, bool clamp = true)
	{
		if(clamp)
			multiplier = Mathf.Clamp(multiplier, 0, 1f);
		//TODO: less questionable ship rbody link
		ship.rBody.AddForceAtPosition(GetForceVector()* multiplier * Time.deltaTime, GetForcePostion(),ForceMode.Impulse);
		///DEBUG
		mat.color = new Color(1, 1- multiplier, 1- multiplier);
	}

	private void Update()
	{
		///DEBUG
		Vector3 postion = GetForcePostion();
		Debug.DrawLine(postion, postion + GetForceVector(), Color.yellow);

		var c = mat.color;
		float r, g, b;
		r = Mathf.Min(1, c.r + .6f * Time.deltaTime);
		g = Mathf.Min(1, c.g + .6f * Time.deltaTime);
		b = Mathf.Min(1, c.b + .6f * Time.deltaTime);

		mat.color = new Color(r,g,b);
	}
}
