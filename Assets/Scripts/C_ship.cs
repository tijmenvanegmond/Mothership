using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class C_ship : MonoBehaviour {

    public GameObject meshGO;
    public Dictionary<int, Node> nodes = new Dictionary<int, Node>();
    
    void AddToNodes(Node aNode)
    {
        nodes.Add(aNode.id,aNode);
    }
       
    private int idCount = 0;
    int getUniqueId()
    {
        idCount = idCount + 1;
        return idCount-1;
    }

    PrismaNode core;
    // Use this for initialization
    void Start () {
        core = new PrismaNode(getUniqueId());
        core.localSpawnPos = Vector3.zero;
        //les create a ship
        CubeNode node1 = new CubeNode(getUniqueId());
        CubeNode node2 = new CubeNode(getUniqueId());
        core.SetConnection(2, node1, 0);
        core.SetConnection(3, node2, 0);

        PrismaNode node1a = new PrismaNode(getUniqueId());
        PrismaNode node2a = new PrismaNode(getUniqueId());
        node1.SetConnection(2, node1a, 0);
        node2.SetConnection(2, node2a, 0);
        AddToNodes(node1a);
        AddToNodes(node2a);

        PrismaNode node1b = new PrismaNode(getUniqueId());
        PrismaNode node2b = new PrismaNode(getUniqueId());
        node1a.SetConnection(2, node1b, 0);
        node2a.SetConnection(4, node2b, 0);
        AddToNodes(node1b);
        AddToNodes(node2b);

        CubeNode node3 = new CubeNode(getUniqueId());      
        core.SetConnection(4,node3,0);

        CubeNode node4 = new CubeNode(getUniqueId());
        CubeNode node5 = new CubeNode(getUniqueId());
        CubeNode node6 = new CubeNode(getUniqueId());

        node3.SetConnection(1, node4, 0);
        node3.SetConnection(2, node5, 0);
        node3.SetConnection(0, node6, 0);

        CubeNode node7 = new CubeNode(getUniqueId());
        node5.SetConnection(2, node7, 0);

        CubeNode node8 = new CubeNode(getUniqueId());
        node7.SetConnection(2, node8, 0);

        AddToNodes(core);
        AddToNodes(node1);
        AddToNodes(node2);
        AddToNodes(node3);
        AddToNodes(node4);
        AddToNodes(node5);
        AddToNodes(node6);
        AddToNodes(node7);
        AddToNodes(node8);
        GenerateShip();

    }

    void GenerateNode(Node node, Node nodeConnection, int connection)
    {       
    }
       
    void GenerateShip()
    {
        Queue<Node> nodeQueue = new Queue<Node>();
        nodeQueue.Enqueue(core);

        //draw all nodes that are connected to the core
        while (nodeQueue.Count > 0) 
        {
            Node curNode = nodeQueue.Dequeue();           
            GameObject newNodeGO = Instantiate(meshGO, transform) as GameObject;
            //newNodeGO.transform.parent = gameObject.transform;
            newNodeGO.transform.localPosition = curNode.localSpawnPos;
            newNodeGO.transform.localRotation = curNode.localRotation;
            curNode.rendered = true;
            for (int i = 0; i < curNode.connections.Length; i++)
            {
               int connID = curNode.connections[i];
               Node connNode = nodes[connID];
                if (!connNode.rendered)
                {
                    Vector3 vector = curNode.getConnectionPos(i);
                    //TODO: support prismas/allow spesific rotations (so its the same no matter the approach)
                    //set the spawn postion as the curnodes pos + the conenction postion and 1normal rotated along curnodes rotation
                    connNode.localSpawnPos = curNode.localSpawnPos + curNode.localRotation * (vector + vector.normalized/2);
                    //rotate the node to look along the connection vector
                    connNode.localRotation = curNode.localRotation * Quaternion.LookRotation(vector); 
                    nodeQueue.Enqueue(connNode);
                }
            }            
        }      
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
