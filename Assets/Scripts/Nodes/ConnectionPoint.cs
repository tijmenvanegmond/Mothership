using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ConnectionPoint
{
	public int Index;
	public Transform Transform;
	public int TypeID;
	public ShipPart Connection;
}
