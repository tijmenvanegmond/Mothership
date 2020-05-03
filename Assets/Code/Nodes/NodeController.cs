using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeController : MonoBehaviour {

    public List<Node> nodeList;

    public List<ConnectionPointType> connectionPointTypes;
    private static Dictionary<int, Node> NodeDict { get; set; }
    private static Dictionary<int, ConnectionPointType> PortTypeDict { get; set; }
    public static int BuildMask { get; private set; }
    public static int BuildLayer { get; private set; }

    public static Node GetNode(int ID) {
        return NodeDict[ID];
    }

    public static ConnectionPointType GetPortType(int ID) {
        return PortTypeDict[ID];
    }

    public void Awake() {
        BuildMask = LayerMask.GetMask("Building");
        BuildLayer = LayerMask.NameToLayer("Building");

        //Load Nodes
        if (nodeList.Any(x => x.Name == null)) {
            var nameClash = nodeList.First(x => x.Name == null).name;
            throw new Exception("All nodes must have a name. " + nameClash + " is missing a name.");
        }

        if (nodeList.GroupBy(x => x.Name).Any(g => g.Count() > 1)) {
            var nameClash = nodeList.GroupBy(x => x.Name).First(g => g.Count() > 1).First().Name;
            throw new Exception("All nodes must have a unique name. Node name clash : " + nameClash + "\n");
        }
        NodeDict = new Dictionary<int, Node>();

        for (int i = 0; i < nodeList.Count; i++) {
            nodeList[i].ID = i;
            NodeDict.Add(i, nodeList[i]);
        }

        //Load connectionpointTypes/portTypes
        if (connectionPointTypes.GroupBy(x => x.ID).Any(g => g.Count() > 1))
            throw new Exception("Not all ConnectionPointTypes have a unique id");

        PortTypeDict = new Dictionary<int, ConnectionPointType>();

        foreach (var portType in connectionPointTypes)
            PortTypeDict.Add(portType.ID, portType);
    }
}