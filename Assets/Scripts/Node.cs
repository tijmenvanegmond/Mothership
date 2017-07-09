using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{   
   public bool rendered;
    public int id;    
    public int[] connections;
    public string type = "node";
    public Vector3 localSpawnPos;
    public Quaternion localRotation = Quaternion.identity;
    //public Transform[] ports;
    public IDictionary<string, Transform> ports;/// = new Dictionary<string, Transform>();

    public virtual Vector3 GetConnectionPos(int x)
    {
        return Vector3.zero;
    }

    public virtual Vector3 GetPosConnected(int id)
    {        
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i] == id)
            {
                return GetConnectionPos(i);
            }
        }
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
