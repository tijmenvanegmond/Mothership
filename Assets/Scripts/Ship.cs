using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class Ship : MonoBehaviour
	{
		Dictionary<int, Node> nodes;
		Rigidbody rBody;

		public void Start()
		{
			nodes = NodeController.NodeDict;
			rBody = GetComponent<Rigidbody>();
			if(rBody == null)
			{
				rBody = gameObject.AddComponent<Rigidbody>() as Rigidbody;
				rBody.drag = 0;
				rBody.angularDrag = .3f;
				rBody.useGravity = false;
				rBody.interpolation = RigidbodyInterpolation.Interpolate;
				rBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
		}

		public bool AddNode(GameObject newNode)
		{
			var nodeComponent = newNode.GetComponent<Node>();
			if (nodeComponent == null)
				return false;
			return AddNode(nodeComponent);
		}

		public bool AddNode(Node newNode)
		{
			var newNodeGO = newNode.gameObject;
			//Check node placement legit
			//TODO: check for collisions
			if (newNodeGO.transform.parent == transform && newNode.HasViableConnections())
			{
				newNode.ConnectPorts();
				return true;
			}
			Debug.Log("Node placement failed. \n Node not a child of expected ship or no viable connections.");
			Destroy(newNodeGO);
			return false;
		}

		public void RemoveNode(Node node)
		{
			//check node connections
			var nodeGroupList = node.DoBridgeCheck().OrderByDescending(g => g.Count()); ;
			if (nodeGroupList.Count() > 1)
			{
				var biggest = nodeGroupList.First();
				var smallest = nodeGroupList.Last();
				//TODO: split up ship
				foreach (var aNode in smallest)
				{
					aNode.Remove();
				}
			}
			node.Remove();
		}


	}
}
