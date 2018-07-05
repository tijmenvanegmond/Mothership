using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	public virtual ConnectionPoint GetConnectionPoint(int connectionNumber)
	{
		return portCollection[connectionNumber];
	}

	public virtual int GetConnectionPointID(GameObject portGO)
	{
		for (int i = 0; i < portCollection.Length; i++)
		{
			if (portCollection[i].Transform.gameObject == portGO)
				return i;
		}
		return 0;
	}

	public virtual Vector3 GetConnectionPos(int connectionNumber)
	{
		return portCollection[connectionNumber].Transform.position;
	}

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

	public void Start() //TODO: default portbuildcollider system
	{
		//var portBuildCollider = new SphereCollider();
		//portBuildCollider.radius = .3f;
		//portBuildCollider.gameObject.layer = LayerMask.NameToLayer("Building");

		//foreach (var port in ports)
		//{
		//	var portGO=Instantiate(portBuildCollider.gameObject, port.Transform);
		//	portGO.transform.localPosition = Vector3.zero;
		//}
	}
}
