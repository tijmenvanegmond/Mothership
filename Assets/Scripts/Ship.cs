using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Ship : MonoBehaviour
	{
		Dictionary<int, Node> nodes;

		public void Start()
		{
			nodes = NodeController.NodeDict;
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

			node.Remove();
		}


	}
}
