using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortType
{
    square_1x1, circle, triangle_right, triangle_equal
}

public class Port : MonoBehaviour {

    public Transform Transform { get; set; }
    public PortType PortType { get; set; }

}
