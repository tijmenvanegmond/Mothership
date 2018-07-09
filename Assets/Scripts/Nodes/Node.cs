using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Scripts;

public abstract class ShipPart : MonoBehaviour
{
}

public class Node : ShipPart
{
	public int ID;
	public string Name;
	[HideInInspector]
	public bool Rendered;
	//Gameobject
	public GameObject BuildPreviewCollider;
	[SerializeField]
	private ConnectionPoint[] portCollection;

	public bool HasPortOfType(int typeID)
	{
		return portCollection.Any(x => x.TypeID == typeID);
	}

	public bool HasMatchingPort(GameObject portGO)
	{
		return portCollection.Any(x => x.Transform.gameObject == portGO);
	}

	public ConnectionPoint GetMatchingPort(GameObject portGO)
	{
		return portCollection.First(x => x.Transform.gameObject == portGO);
	}

	public IEnumerable<ConnectionPoint> GetPortsOfType(int id)
	{
		return portCollection.Where(x => x.TypeID == id);
	}

	public void Start()
	{
		//Add a default portBuildCollider to each port (of node)
		var defaultPortColliderGO = new GameObject();
		var defaultPortBuildCollider = defaultPortColliderGO.AddComponent<BoxCollider>() as BoxCollider;
		defaultPortBuildCollider.center = new Vector3(0, .02f,0);
		defaultPortBuildCollider.size = new Vector3(.5f, .04f, .5f);
		defaultPortColliderGO.layer = NodeController.BuildLayer;
		defaultPortColliderGO.name = "portBuildCollider";

		foreach (var port in portCollection)
		{
			if (port.Transform.childCount != 0)
				continue;
			var portGO = Instantiate(defaultPortColliderGO, port.Transform);
			portGO.transform.localPosition = Vector3.zero;
		}
	}

	/// <summary>
	/// Casts a overlap sphere for each connection to see if it there is a port of the same type close enough
	/// </summary>
	/// <returns></returns>
	internal bool HasViableConnections() //TODO: implement required connections
	{
		var hitColliders = new Collider[10];
		foreach (var port in portCollection)
		{
			var amount = Physics.OverlapSphereNonAlloc(port.Transform.position, .01f, hitColliders, NodeController.BuildMask);
			for (var i = 0; i < amount; i++)
			{
				var hitCollider = hitColliders[i];
				var hitNodeGO = Utility.FindParentWithTag(hitCollider.gameObject, "Node");
				if (hitNodeGO == null)
					continue;

				var hitNode = hitNodeGO.GetComponent<Node>();
				var hitPortInfo = hitNode.GetMatchingPort(hitCollider.gameObject);
				if (port.TypeID != hitPortInfo.TypeID)
					continue;
				if (hitPortInfo.Connection != null)
					return true;
			}
		}

		return false;
	}

	internal void ConnectPorts()
	{
		throw new NotImplementedException();
	}

}
