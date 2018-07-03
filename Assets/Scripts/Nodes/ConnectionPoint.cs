using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ConnectionPoint
{
	public Transform Transform;
	public int TypeID;
	public ShipPart Connection;
}
