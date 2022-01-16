using System;
using UnityEngine;

[Serializable]
public struct ConnectionPoint {
    [HideInInspector]
    public int Index;
    public Transform Transform;
    public int TypeID;
    public Node Connection;
}
