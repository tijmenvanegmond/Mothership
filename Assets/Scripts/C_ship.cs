using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class C_ship : MonoBehaviour {

    public GameObject cubeGO;
    public GameObject prismaGO;
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
        AddToNodes(core);

        ////les create a ship
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
            GameObject newNodeGO;
            if (curNode.type == "prisma")
            {
                newNodeGO = Instantiate(prismaGO, transform) as GameObject;
            }
            else
            {
                newNodeGO = Instantiate(cubeGO, transform) as GameObject;
            }
            newNodeGO.transform.localPosition = curNode.localSpawnPos;
            newNodeGO.transform.localRotation = curNode.localRotation;
            curNode.rendered = true;
            for (int i = 0; i < curNode.connections.Length; i++)
            {
                int connID = curNode.connections[i];
                Node connNode = nodes[connID];
                if (!connNode.rendered)
                {
                    Vector3 curConPos = curNode.GetConnectionPos(i)*2;// connNode.GetPosConnected(curNode.id);
                    //TODO: support prismas/allow spesific rotations (so its the same no matter the approach)
                    //set the spawn postion as the curnodes pos + the conenction postion and 1normal rotated along curnodes rotation 
                    connNode.localSpawnPos = curNode.localSpawnPos + curNode.localRotation * curConPos;
                    //rotate the node to look along the connection vector
                    connNode.localRotation = curNode.localRotation * Quaternion.LookRotation(curConPos); 
                    nodeQueue.Enqueue(connNode);
                }
            }            
        }      
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
