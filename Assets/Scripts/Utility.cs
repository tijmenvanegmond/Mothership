using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public static GameObject FindChild(GameObject parent, string name)
    {
        foreach (GameObject child in parent.transform)
        {
            if (child.name == name)
                return child;
        }
        return null;
    }

    public static void ChildrenSetActive(GameObject gameObject, bool value = false)
    {
        foreach (GameObject child in gameObject.transform)
        {
            gameObject.SetActive(value);
        }
    }

	/// <summary>
	/// Changes the nodes transform so that both ports are connected
	/// </summary>
	/// <param name="from"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	public static GameObject ConnectPortToTarget(GameObject node,Transform port,Transform target)
	{
		var t = node.transform;
		var pivot = new GameObject().transform;
		t.rotation = Quaternion.identity;
		t.position = Vector3.zero;
		t.parent = pivot;
		t.localRotation = Quaternion.Inverse(port.localRotation);
		t.Translate(-port.localPosition, Space.Self);
		pivot.transform.rotation = target.rotation * Quaternion.Euler(0, 0, 180f); //inverse rotation so port ar facing eachother
		pivot.transform.position = target.position;
		t.transform.parent = null;
		Destroy(pivot.gameObject);
		return node;
	}
}
