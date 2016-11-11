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

        CubeNode node1 = new CubeNode(getUniqueId());
        CubeNode node2 = new CubeNode(getUniqueId());
        CubeNode node3 = new CubeNode(getUniqueId());

        core.SetConnection(2,node1,0);
        core.SetConnection(3,node2,0);
        core.SetConnection(4,node3,0);

        CubeNode node4 = new CubeNode(getUniqueId());
        CubeNode node5 = new CubeNode(getUniqueId());
        CubeNode node6 = new CubeNode(getUniqueId());

        node3.SetConnection(1, node4, 0);
        node3.SetConnection(2, node5, 0);
        node3.SetConnection(5, node6, 0);

        AddToNodes(core);
        AddToNodes(node1);
        AddToNodes(node2);
        AddToNodes(node3);
        AddToNodes(node4);
        AddToNodes(node5);
        AddToNodes(node6);
        GenerateShip();

    }

    void GenerateNode(Node node, Node nodeConnection, int connection)
    {       
    }

   
    void GenerateShip()
    {
        Queue<Node> nodeQueue = new Queue<Node>();
        nodeQueue.Enqueue(core);

        while (nodeQueue.Count > 0)
        {
            Node curNode = nodeQueue.Dequeue();           
            GameObject newNodeGO = Instantiate(meshGO, transform) as GameObject;
            //newNodeGO.transform.parent = gameObject.transform;
            newNodeGO.transform.localPosition = curNode.localSpawnPos;
            newNodeGO.transform.localRotation = Quaternion.identity;
            curNode.rendered = true;
            for (int i = 0; i < curNode.connections.Length; i++)
            {
               int connID = curNode.connections[i];
               Node connNode = nodes[connID];
                if (!connNode.rendered)
                {
                    connNode.localSpawnPos = curNode.localSpawnPos + curNode.getConnectionPos(i);
                    nodeQueue.Enqueue(connNode);
                }
            }            
        }      
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
