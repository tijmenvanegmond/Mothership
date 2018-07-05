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

		public bool AddNode(Node baseNode, int baseNodePortNumber, Node newNode, int newNodePortNumber, int rotation = 0)
		{
			var basePort = baseNode.GetConnectionPoint(baseNodePortNumber);
			var newPort = newNode.GetConnectionPoint(newNodePortNumber);
			var newPortType = NodeController.PortTypeDict[newPort.TypeID];

			//TODO: checknodeplacement legit
			//return false
			//Placenode
			//Calc node placement postion based on portPostions TODO: fix rotation/translation issues
			var newNodeGO = Instantiate(newNode.gameObject) as GameObject;
			Utility.ConnectPortToTarget(newNodeGO, newPort.Transform, basePort.Transform);
			newNodeGO.transform.Rotate(newPort.Transform.localPosition, rotation * newPortType.RotationStep, Space.Self); //Rotate among port axis by (player)custom rotation
			newNodeGO.transform.parent = transform;

			return true;
		}

		public void RemoveNode(GameObject node)
		{
			//GameObject port = Utility.FindParentWithTag(node.gameObject, "Port");
			//GameObject parentNode = Utility.FindParentWithTag(port, "Node");
			Destroy(node.gameObject);
		}
	}
}
