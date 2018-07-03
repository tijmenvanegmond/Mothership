using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class ShipPart : MonoBehaviour
{
}

public class Node : ShipPart
{
	public bool rendered;
	public int id;
	public ConnectionPoint[] ConnectionPoints;
	public Mesh PreviewMesh;

	public virtual Vector3 GetConnectionPos(int connectionNumber)
	{
		return ConnectionPoints[connectionNumber].Transform.position;
	}

	//public virtual Vector3 GetPosConnected(int id)
	//{
	//	for (int i = 0; i < ConnectionPoints.Length; i++)
	//	{
	//		if (ConnectionPoints[i].Connection == id)
	//		{
	//			return GetConnectionPos(i);
	//		}
	//	}
	//	return Vector3.zero;
	//}

	public override bool Equals(object obj)
	{
		//Check for null and compare run-time types.
		if ((obj == null) || !this.GetType().Equals(obj.GetType()))
		{
			return false;
		}
		else
		{
			Node i = (Node)obj;
			return id == i.id;
		}
	}
	public override int GetHashCode()
	{
		return id.GetHashCode();
	}
}
