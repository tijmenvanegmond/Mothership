using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    public bool rendered;
    public int id;
    public int[] connections;
    public Vector3 localSpawnPos;
    public Quaternion localRotation = Quaternion.identity;

    public virtual Vector3 getConnectionPos(int x)
    {
        return Vector3.zero;
    }

    protected virtual int GetConnection(int connectionSlot)
    {
        if (connections.Length > connectionSlot)
        {
            return connections[connectionSlot];
        }
        else
        {
           
            print("Node does not have that connection slot");
            return 0;
        }
    }

    protected virtual void SetConnection(int connectionSlot,int spec)
    {
        if (connections.Length > connectionSlot)
        {
            connections[connectionSlot] = spec;
        }
        else
        {
            print("Node does not have that connection slot");
        }
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types. 
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Node i = (Node)obj;
            return id == i.id;
        }
    }

    public override int GetHashCode()
    {
        return id;
    }

}
