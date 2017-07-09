using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;   

public class CreateTriangle : MonoBehaviour {

    public bool Rendered = false;
    public bool Prisma = true;
    public float Size = 1f;
    public float Thickness = 0.1f;
    public float Height = 0.1f;
    private List<Vector3> _newVertices = new List<Vector3>();    
    private int[] _newTriangles = new int[24]{
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
        if (!Rendered)
        {
            if (Prisma)
            {
                _newVertices.Add(new Vector3(0, Height / 2f, 0.5773F));
                _newVertices.Add(new Vector3(.5f, Height / 2f, -.2887f));
                _newVertices.Add(new Vector3(-.5f, Height / 2f, -.2887f));
                _newVertices.Add(new Vector3(0, -Height / 2f, 0.5773f));
                _newVertices.Add(new Vector3(.5f, -Height / 2f, -.2887f));
                _newVertices.Add(new Vector3(-.5f, -Height / 2f, -.2887f));
            }
            else
            {
              
                _newVertices.Add(new Vector3( Thickness / 2f,  Size / 2f, -Size / 2f));
                _newVertices.Add(new Vector3( Thickness / 2f, -Size / 2f,  Size / 2f));
                _newVertices.Add(new Vector3( Thickness / 2f, -Size / 2f, -Size / 2f));
                _newVertices.Add(new Vector3(-Thickness / 2f,  Size / 2f, -Size / 2f));
                _newVertices.Add(new Vector3(-Thickness / 2f, -Size / 2f,  Size / 2f));
                _newVertices.Add(new Vector3(-Thickness / 2f, -Size / 2f, -Size / 2f));
            }


            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = _newVertices.ToArray();
            //mesh.normals = normals;
            mesh.triangles = _newTriangles;
            mesh.RecalculateNormals();
            Rendered = true;
            MeshCollider meshc = GetComponent(typeof(MeshCollider)) as MeshCollider;
            meshc.sharedMesh = mesh; // Give it your mesh here.
        }
    }
}
