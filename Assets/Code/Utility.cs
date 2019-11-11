using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    public static GameObject FindParentWithTag (GameObject childObject, string tag) {
        Transform t = childObject.transform;
        while (t.parent != null) {
            if (t.parent.tag == tag) {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public static GameObject FindChild (GameObject parent, string name) {
        foreach (GameObject child in parent.transform) {
            if (child.name == name)
                return child;
        }
        return null;
    }

    public static void ChildrenSetActive (Transform parentTransform, bool value = false) {
        foreach (Transform child in parentTransform)
            child.gameObject.SetActive (value);
    }
}
