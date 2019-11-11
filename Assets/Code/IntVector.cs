using System.Collections;
using UnityEngine;

//public class Vector3
//{
//	public override Vector3 (IntVector3 vector)
//	{	
//		this.x = (float)vector.x;
//		this.y = (float)vector.y;
//		this.z = (float)vector.z;
//	}
//}

///<summary>Repersentation of an 3D interger vector.
///</summary> 
public class IntVector3 {
    public int x;
    public int y;
    public int z;

    public Vector3 vector3 {
        get {
            return new Vector3 ((float) x, (float) y, (float) z);
        }
    }
    public IntVector3 oneBack {
        get { return new IntVector3 (x, y, z - 1); }
    }
    public IntVector3 oneForward {
        get { return new IntVector3 (x, y, z + 1); }
    }
    public IntVector3 oneRight {
        get { return new IntVector3 (x + 1, y, z); }
    }
    public IntVector3 oneLeft {
        get { return new IntVector3 (x - 1, y, z); }
    }
    public IntVector3 oneDown {
        get { return new IntVector3 (x, y - 1, z); }
    }
    public IntVector3 oneUp {
        get { return new IntVector3 (x, y + 1, z); }
    }

    static public IntVector3 back {
        get { return new IntVector3 (0, 0, -1); }
    }
    static public IntVector3 front {
        get { return new IntVector3 (0, 0, 1); }
    }
    static public IntVector3 right {
        get { return new IntVector3 (1, 0, 0); }
    }
    static public IntVector3 left {
        get { return new IntVector3 (-1, 0, 0); }
    }
    static public IntVector3 down {
        get { return new IntVector3 (0, 1, 0); }
    }
    static public IntVector3 up {
        get { return new IntVector3 (0, 1, 0); }
    }

    public IntVector3 (int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public IntVector3 (float x, float y, float z) {
        this.x = Mathf.RoundToInt (x);
        this.y = Mathf.RoundToInt (y);
        this.z = Mathf.RoundToInt (z);
    }

    public IntVector3 (Vector3 vector) {
        this.x = Mathf.RoundToInt (vector.x);
        this.y = Mathf.RoundToInt (vector.y);
        this.z = Mathf.RoundToInt (vector.z);
    }

    public static IntVector3 operator + (IntVector3 v1, IntVector3 v2) //does it work?
    {
        return new IntVector3 (v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static IntVector3 operator - (IntVector3 v1, IntVector3 v2) //does it work?
    {
        return new IntVector3 (v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static IntVector3 operator * (IntVector3 v1, IntVector3 v2) {
        return new IntVector3 (v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static IntVector3 operator * (IntVector3 v1, int i2) {
        return new IntVector3 (v1.x * i2, v1.y * i2, v1.z * i2);
    }

    public override bool Equals (object obj) {
        //Check for null and compare run-time types. 
        if ((obj == null) || !this.GetType ().Equals (obj.GetType ())) {
            return false;
        } else {
            IntVector3 v = (IntVector3) obj;
            return (x == v.x) && (y == v.y) && (z == v.z);
        }
    }

    public override int GetHashCode () {
        return x ^ y ^ z;
    }

    public override string ToString () {
        return ("( " + x + ", " + y + ", " + z + " )");
    }

    //	public override Serialize()
    //	{}

}