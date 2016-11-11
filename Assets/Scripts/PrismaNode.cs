using UnityEngine;
using System.Collections;

public class PrismaNode : Node {
    
    private static Vector3[] connectionPositions = new Vector3[] {  Vector3.up,
                                                                    Vector3.down,
                                                                    new Vector3((float).375, 0, (float).216),
                                                                    new Vector3(0, 0, (float)-.43301),
                                                                    new Vector3((float)-.375,0, (float).216)}; //core,up, down, 60, 180, 300 
    public override Vector3 getConnectionPos(int connectionNumber)
    {
        return connectionPositions[connectionNumber];
    }

    public PrismaNode(int newId) {
        connections = new int[5];
        id = newId;        
    }

    public void SetConnection(int connectionSlot, Node node, int otherConnectionSlot)
    {
        connections[connectionSlot] = node.id;
        node.connections[otherConnectionSlot] = id;
    }
    
  }
