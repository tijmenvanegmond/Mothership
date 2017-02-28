using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;   

public class CreateTriangle : MonoBehaviour {

    public bool rendered = false;
    public float height = 0.1f;
    private List<Vector3> newVertices = new List<Vector3>();    
    private int[] newTriangles = new int[24]{
            0, 1, 2,
            3, 5, 4,

            0, 3, 4,
            1, 0, 4,

            1, 4, 5,
            2, 1, 5,

            2, 5, 3,
            0, 2, 3,
        };
    //public Vector3[] normals  = new Vector3[6] {Vector3.forward, Vector3.forward, Vector3.forward, Vector3.back, Vector3.back, Vector3.back };
   
    void Start() {
        if (!rendered)
        {
            newVertices.Add(new Vector3(0, height/2f, 0.5773F));
            newVertices.Add(new Vector3(.5f, height / 2f, -.2887f));
            newVertices.Add(new Vector3(-.5f, height / 2f, -.2887f));
            newVertices.Add(new Vector3(0, -height / 2f, 0.5773f));
            newVertices.Add(new Vector3(.5f, -height / 2f, -.2887f));
            newVertices.Add(new Vector3(-.5f, -height / 2f, -.2887f));

            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = newVertices.ToArray();
            //mesh.normals = normals;
            mesh.triangles = newTriangles;
            mesh.RecalculateNormals();
            rendered = true;
            MeshCollider meshc = GetComponent(typeof(MeshCollider)) as MeshCollider;
            meshc.sharedMesh = mesh; // Give it your mesh here.
        }
    }
}
