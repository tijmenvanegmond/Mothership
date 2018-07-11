using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class BuildShip : MonoBehaviour
	{
		private Node selectedNode;
		public int Rotation = 0;
		public int PortNumber = 0;
		public int SelectedNodeID = 0;

		private IDictionary<int, Node> nodes;
		private IDictionary<int, ConnectionPointType> portTypes;

		private GameObject CursorGO;

		private Ship hitShip;
		private Node hitNode; //selected node to build on / delete
		private ConnectionPoint hitPort; //selected port to build on / delete
		private ConnectionPointType hitPortType { get { return portTypes[hitPort.TypeID]; } }

		void Start()
		{
			nodes = NodeController.NodeDict;
			portTypes = NodeController.PortTypeDict;

			UpdateCursorShape(0);
		}

		//Update inputs once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha0))
				UpdateCursorShape(0);
			else if (Input.GetKeyDown(KeyCode.Alpha1))
				UpdateCursorShape(1);
			else if (Input.GetKeyDown(KeyCode.Alpha2))
				UpdateCursorShape(2);
			else if (Input.GetKeyDown(KeyCode.Alpha3))
				UpdateCursorShape(3);
			else if (Input.GetKeyDown(KeyCode.Alpha4))
				UpdateCursorShape(4);
			else if (Input.GetKeyDown(KeyCode.Alpha5))
				UpdateCursorShape(5);
			else if (Input.GetKeyDown(KeyCode.Alpha6))
				UpdateCursorShape(6);
			else if (Input.GetKeyDown(KeyCode.Alpha7))
				UpdateCursorShape(7);
			else if (Input.GetKeyDown(KeyCode.Alpha8))
				UpdateCursorShape(8);
			else if (Input.GetKeyDown(KeyCode.Alpha9))
				UpdateCursorShape(9);

			if (Input.GetButtonDown("Rotate"))
				Rotation++;

			if (Input.GetButtonDown("SwitchPort"))
				PortNumber++;
		}

		void UpdateCursorShape(int nodeID)
		{
			SelectedNodeID = nodeID;
			selectedNode = nodes[nodeID];

			if (CursorGO != null)
				Destroy(CursorGO);

			CursorGO = Instantiate(selectedNode.BuildPreviewCollider);
			CursorGO.AddComponent<Cursor>();
			CursorGO.SetActive(false);
			CursorGO.name = "CURSOR";
		}

		void FixedUpdate()
		{
			var layerMask = NodeController.BuildMask;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, layerMask);

			//make sure there are no nulls
			if (!IsValidShipHit(hit))//only continue if a viable (with port & ship parent) collider has been hit
			{
				CursorGO.SetActive(false);
				return;
			}

			if (!selectedNode.HasPortOfType(hitPort.TypeID))
			{
				Debug.Log("SelectedNode does not have a port of a matching type");
				CursorGO.SetActive(false);
				return;
			}

			//now assumed there are no nulls
			Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
			if (Input.GetButtonDown("Fire2")) // if right mouse button was pressed this update delete node that was hit
				hitShip.RemoveNode(hitNode);
			else if (Input.GetButtonDown("Fire1")) // if left mouse button was pressed this update create a node
			{
				UpdateCursorPlacement();
				if (CursorGO.GetComponent<Cursor>().IsFree)
				{
					var nodeGO = Instantiate(selectedNode.gameObject, hitShip.transform);
					nodeGO.transform.position = CursorGO.transform.position;
					nodeGO.transform.rotation = CursorGO.transform.rotation;
					hitShip.AddNode(nodeGO);
				}
				else
					Debug.Log("Object:" + CursorGO.GetComponent<Cursor>().Obstruction + "\n Is obstucting");
			}
			else
			{
				UpdateCursorPlacement();
			}
		}

		private bool IsValidShipHit(RaycastHit hit)
		{
			if (hit.collider == null)
				return false;
			GameObject shipGameObject = Utility.FindParentWithTag(hit.collider.gameObject, "Ship");
			if (shipGameObject == null)
				return false;
			hitShip = shipGameObject.GetComponent<Ship>();
			if (hitShip == null)
				return false;
			hitNode = Utility.FindParentWithTag(hit.collider.gameObject, "Node").GetComponent<Node>();
			if (hitNode == null)
				return false;
			var portGO = Utility.FindParentWithTag(hit.collider.gameObject, "Port");
			if (portGO == null)
				return false;
			if (!hitNode.HasMatchingPort(portGO))
			{
				Debug.Log("Could not match portGO to port");
				return false;
			}
			hitPort = hitNode.GetMatchingPort(portGO);

			return true;
		}

		private void UpdateCursorPlacement()
		{
			//Calc node placement postion based on portPostions
			var newPort = GetSelectedPort();
			var t = CursorGO.transform;
			var pivot = new GameObject().transform;
			t.rotation = Quaternion.identity;
			t.position = Vector3.zero;
			t.parent = pivot;
			t.localRotation = Quaternion.Inverse(newPort.Transform.localRotation);
			t.Translate(-newPort.Transform.localPosition, Space.Self);
			pivot.transform.rotation = hitPort.Transform.rotation * Quaternion.Euler(0, 0, 180f); //inverse rotation so port ar facing eachother
			pivot.transform.position = hitPort.Transform.position;
			t.transform.parent = null;
			Destroy(pivot.gameObject);

			CursorGO.transform.RotateAround(hitPort.Transform.position, hitPort.Transform.rotation * Vector3.up, Rotation * hitPortType.RotationStep);
			CursorGO.gameObject.SetActive(true);
		}

		/// <summary>
		/// Changes the nodes transform so that both ports are connected
		/// </summary>
		/// <param name="from"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static GameObject ConnectPortToTarget(GameObject node, Transform port, Transform target)
		{
			var t = node.transform;
			var pivot = new GameObject().transform;
			t.rotation = Quaternion.identity;
			t.position = Vector3.zero;
			t.parent = pivot;
			t.localRotation = Quaternion.Inverse(port.localRotation);
			t.Translate(-port.localPosition, Space.Self);
			pivot.transform.rotation = target.rotation * Quaternion.Euler(0, 0, 180f); //inverse rotation so port ar facing eachother
			pivot.transform.position = target.position;
			t.transform.parent = null;
			Destroy(pivot.gameObject);
			return node;
		}


		/// <summary>
		/// get info of the current selected port of the selectednode (breaks on no matching porttypes)
		/// </summary>
		/// <returns></returns>
		private ConnectionPoint GetSelectedPort()
		{
			var matchingPorts = selectedNode.GetPortsOfType(hitPortType.ID);
			var amount = matchingPorts.Count();
			return matchingPorts.ElementAt(PortNumber % amount);
		}

	}
}

