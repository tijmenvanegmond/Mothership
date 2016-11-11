using UnityEngine;
using System.Collections;

public class CubeNode : Node {
    
    private static Vector3[] connectionPositions = new Vector3[] { Vector3.up/2, Vector3.down/2, Vector3.forward / 2, Vector3.right/2,  Vector3.back/2, Vector3.left / 2, }; //up, down, 0, 90, 180, 270     
    public override Vector3 getConnectionPos(int connectionNumber)
    {
        return connectionPositions[connectionNumber];
    }

    public CubeNode(int newId) {
        connections = new int[6];
        id = newId;        
    }

    public void SetConnection(int connectionSlot, Node node, int otherConnectionSlot)
    {
        connections[connectionSlot] = node.id;
        node.connections[otherConnectionSlot] = id;
    }
    
  }
