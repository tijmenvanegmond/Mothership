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

			//TODO: check node placement legit
			if (newNodeGO.transform.parent != gameObject)
				return false;

			if (!newNode.HasViableConnections())
				return false;

			newNode.ConnectPorts();
			return true;
		}

		public void RemoveNode(GameObject node)
		{
			//node.DisconnetPorts();
			//GameObject port = Utility.FindParentWithTag(node.gameObject, "Port");
			//GameObject parentNode = Utility.FindParentWithTag(port, "Node");
			Destroy(node.gameObject);
		}
	}
}
