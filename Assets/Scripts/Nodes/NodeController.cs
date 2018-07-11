using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
	public class NodeController : MonoBehaviour
	{
		public List<Node> nodeList;
		public List<Panel> panelList;
		public List<ConnectionPointType> connectionPointTypes;

		public static Dictionary<int, Node> NodeDict { get; private set; }
		public static Dictionary<int, Panel> PanelDict { get; private set; }
		public static Dictionary<int, ConnectionPointType> PortTypeDict { get; private set; }
		public static int BuildMask;
		public static int BuildLayer;

		public void Awake()
		{
			BuildMask = LayerMask.GetMask("Building");
			BuildLayer = LayerMask.NameToLayer("Building");

			//Load Nodes
			if (nodeList.GroupBy(x => x.ID).Any(g => g.Count() > 1))
				throw new Exception("All nodes must have a unique ID");

			if (nodeList.Any(x => x.Name == null))
				throw new Exception("All nodes must have a name");

			if (nodeList.GroupBy(x => x.Name).Any(g => g.Count() > 1))
				throw new Exception("All nodes must have a unique name");

			NodeDict = new Dictionary<int, Node>();

			foreach (var node in nodeList)
				NodeDict.Add(node.ID, node);

			//Load Panels(?)
			if (nodeList.GroupBy(x => x.ID).Any(g => g.Count() > 1))
				throw new Exception("All panels must have a unique ID");

			if (nodeList.Any(x => x.Name == null))
				throw new Exception("All panels must have a name");

			if (nodeList.GroupBy(x => x.Name).Any(g => g.Count() > 1))
				throw new Exception("All panels must have a unique name");

			PanelDict = new Dictionary<int, Panel>();

			foreach (var panel in panelList)
				PanelDict.Add(panel.ID, panel);

			//Load connectionpointTypes/portTypes
			if (connectionPointTypes.GroupBy(x => x.ID).Any(g => g.Count() > 1))
				throw new Exception("Not all ConnectionPointTypes have a unique id");

			PortTypeDict = new Dictionary<int, ConnectionPointType>();

			foreach (var portType in connectionPointTypes)
				PortTypeDict.Add(portType.ID, portType);
		}
	}
}