using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;



public class Ship : MonoBehaviour
{

    Dictionary<string, GameObject> nodes;

    public void Start()
    {
        nodes = NodeController.nodes;
    }

    public bool AddNode(GameObject collider, string type, int rotation = 0)
    {
        GameObject port = Utility.FindParentWithTag(collider.gameObject, "Port");
        GameObject parentNode = Utility.FindParentWithTag(port, "Node");
        if (nodes.ContainsKey(type))
        {
            GameObject node = Instantiate(nodes[type], transform) as GameObject;
            node.transform.rotation = port.transform.rotation;
            node.transform.position = port.transform.position;
            if (type == "prisma" && !(parentNode.name == "prisma(Clone)" && (port.name == "Up" || port.name == "Down"))) //if its a prisma apply a diffrent rotation/translation; except when hovering over another triangle
            {
                node.transform.Rotate(new Vector3(0, rotation, 0));
                node.transform.Translate(Vector3.up * .2887f, Space.Self);
            }
            else
            {
                node.transform.Rotate(Vector3.right * 90f); //no need to rotate since currently all nodes dont change on rotation
                node.transform.Translate(Vector3.back * .5f, Space.Self);

            }
            return true;
        }
        return false;


    }
}
