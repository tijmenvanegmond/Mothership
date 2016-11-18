using UnityEngine;
using System.Collections;

public class PrismaNode : Node {

    public Transform[] connectionTransfroms;
    //private static Vector3[] connectionPositions = new Vector3[] {  Vector3.up,
    //                                                                Vector3.down,
    //                                                                new Vector3(.25f, 0, .14434f),
    //                                                                new Vector3(0, 0, -.2886f),
    //                                                                new Vector3(-.25f,0, .14434f)}; //core,up, down, 60, 180, 300 
    public override Vector3 GetConnectionPos(int connectionNumber)
    {        
        return connectionTransfroms[connectionNumber].position;
    }

    public Transform GetConnectionTrans(int connectionNumber)
    {
        return connectionTransfroms[connectionNumber];
    }

    public PrismaNode(int newId) {
        connections = new int[5];
        type = "prisma";
        id = newId;        
    }

    public void SetConnection(int connectionSlot, Node node, int otherConnectionSlot)
    {
        connections[connectionSlot] = node.id;
        node.connections[otherConnectionSlot] = id;
    }
    
  }
