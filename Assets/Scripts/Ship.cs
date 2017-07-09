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

    public bool AddNode(GameObject portCollider, string type, int rotation = 0)
    {
        GameObject port = Utility.FindParentWithTag(portCollider.gameObject, "Port");
        GameObject parentNode = Utility.FindParentWithTag(port, "Node");
        if (nodes.ContainsKey(type))
        {
            //disable current wall at that port
            //portCollider.SetActive(false);
            //place a new node at the port (as child of ship(this))
            GameObject node = Instantiate(nodes[type], transform) as GameObject;
            node.transform.rotation = port.transform.rotation;
            node.transform.position = port.transform.position;
            if (type == "prisma" && portCollider.name == "PlateSquare")//!(parentNode.name == "prisma(Clone)" && (port.name == "Up" || port.name == "Down"))) //if its a prisma apply a diffrent rotation/translation; except when hovering over another triangle
            {
                node.transform.Translate(Vector3.up * .2887f, Space.Self);
                node.transform.Rotate(new Vector3(270f, 0, rotation * 90f));
            }
            else
            {
                node.transform.Translate(Vector3.up * .5f, Space.Self);
                if (type != "prisma")
                {
                    node.transform.Rotate(new Vector3(0, rotation * 90f, 0));
                }
            }

            //Utility.ChildrenSetActive(node, false);
            //Utility.FindChild(node, "Down").SetActive(false);

            return true;
        }
        return false;
    }

    public void RemoveNode(GameObject node)
    {
        //GameObject port = Utility.FindParentWithTag(node.gameObject, "Port");
        //GameObject parentNode = Utility.FindParentWithTag(port, "Node");

        DestroyObject(node);

    }
}
