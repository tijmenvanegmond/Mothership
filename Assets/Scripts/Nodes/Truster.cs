using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truster : Node {

	public float Trust;
	public Transform[] trusterTransforms;
	//DEBUG
	private Material mat;

	public Vector3 GetForceDirection()
	{
		return transform.up * -1 * transform.localScale.y;
	}

	/// <summary>
	/// worldspace vector of the force this truster delivers at full trust
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
		///DEBUG
		mat = transform.GetChild(2).GetComponent<Renderer>().material;
	}

	public void FireAt(float multiplier)
	{
		//TODO: less questionable ship rbody link
		transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(GetForceVector()* multiplier, GetForcePostion());
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
