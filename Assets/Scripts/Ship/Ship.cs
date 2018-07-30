using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class Ship : MonoBehaviour
	{
		public Rigidbody rBody;
		Dictionary<int, Node> nodes;

		public void Start()
		{
			nodes = NodeController.NodeDict;
			rBody = GetComponent<Rigidbody>();
			if (rBody == null)
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
			//Check node connections
			var nodeSetList = node.DoBridgeCheck().OrderByDescending(g => g.Count()); //biggest first
			if (nodeSetList.Count() > 1)
			{
				//split up the ship into groups created
				foreach (var set in nodeSetList.Skip(1))
				{
					var newShipGO = new GameObject();
					newShipGO.name = name + "-breakoff";
					var newShip = newShipGO.AddComponent<Ship>();
					//TODO: add Velocity & angularVelocity
					foreach (var aNode in set)
						aNode.transform.parent = newShipGO.transform;
				}
			}
			node.Remove();
			//TODO: recalculate ship properties
		}
	}
}
