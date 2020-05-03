using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Construct : MonoBehaviour {
    protected Dictionary<int, Node> nodes;
    protected HashSet<Node> myNodes;

    public virtual void Start() {
        nodes = NodeController.NodeDict;
        myNodes = new HashSet<Node>();

        AddChildNodesToMyNodes();
    }

    private void AddChildNodesToMyNodes() {
        foreach (Transform child in transform)
            AddNode(child.gameObject, false);
    }

    public bool AddNode(GameObject newNode, bool checkPlacement = true) {
        var nodeComponent = newNode.GetComponent<Node>();
        if (nodeComponent == null)
            return false;
        return AddNode(nodeComponent, checkPlacement);
    }

    public bool AddNode(Node newNode, bool checkPlacement = true) {
        if (checkPlacement) {
            //Check node placement legit
            var newNodeGO = newNode.gameObject;
            //TODO: check for collisions
            if (newNodeGO.transform.parent != transform || !newNode.HasViableConnections()) {
                Debug.Log("Node placement failed. \n Node not a child of expected ship or no viable connections.");
                Destroy(newNodeGO);
                return false;
            }
        }
        newNode.ConnectPorts();
        myNodes.Add(newNode);
        return true;
    }

    public void RemoveNode(Node node) {
        //Check node connections
        //TODO: the ship should remain the group where the playerchar is seated
        var nodeSetList = node.CheckIfNodeABridge().OrderByDescending(g => g.Count()); //biggest first
        if (nodeSetList.Count() > 1) {
            HandleBridgeRemoval(nodeSetList);
        }

        myNodes.Remove(node);
        node.Remove();
        //TODO: recalculate properties
        this.UpdateConstruct();
    }

    protected virtual IEnumerable<GameObject> HandleBridgeRemoval(IEnumerable<HashSet<Node>> nodeSetList) {
        //bigest group remains the ship
        myNodes = nodeSetList.First();
        //split up the ship into groups created
        var breakoffParts = new List<GameObject>();
        foreach (var set in nodeSetList.Skip(1)) {
            var newShipGO = new GameObject();
            newShipGO.name = name + "-breakoff";
            var newShip = newShipGO.AddComponent<Ship>();
            foreach (var aNode in set) {
                aNode.transform.parent = newShipGO.transform;
            }
            breakoffParts.Add(newShipGO);
        }
        return breakoffParts;
    }

    protected virtual void UpdateConstruct() {

    }

}