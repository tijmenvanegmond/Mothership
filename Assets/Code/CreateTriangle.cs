using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour {
    public bool Rendered = false;
    public float Size = 1f;
    public float Thickness = 0.1f;
    enum PrismaShapes {
        prisma,
        slope,

        corner_slope,
        custom
    }

    [SerializeField]
    PrismaShapes shape;
    [SerializeField]
    Transform[] corners = new Transform[3];
    private List<Vector3> _newVertices = new List<Vector3> ();
    private int[] _newTriangles = new int[24] {
        0,
        1,
        2,
        3,
        5,
        4,

        0,
        3,
        4,
        1,
        0,
        4,

        1,
        4,
        5,
        2,
        1,
        5,

        2,
        5,
        3,
        0,
        2,
        3,
    };

    void Start () {
        if (!Rendered) {
            switch (shape) {
                case PrismaShapes.custom:
                    _newVertices.Add (new Vector3 (Thickness / 2f, corners[0].localPosition.y, corners[0].localPosition.z));
                    _newVertices.Add (new Vector3 (Thickness / 2f, corners[1].localPosition.y, corners[1].localPosition.z));
                    _newVertices.Add (new Vector3 (Thickness / 2f, corners[2].localPosition.y, corners[2].localPosition.z));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, corners[0].localPosition.y, corners[0].localPosition.z));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, corners[1].localPosition.y, corners[1].localPosition.z));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, corners[2].localPosition.y, corners[2].localPosition.z));
                    break;
                case PrismaShapes.slope:
                    _newVertices.Add (new Vector3 (Thickness / 2f, Size / 2f, -Size / 2f));
                    _newVertices.Add (new Vector3 (Thickness / 2f, -Size / 2f, Size / 2f));
                    _newVertices.Add (new Vector3 (Thickness / 2f, -Size / 2f, -Size / 2f));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, Size / 2f, -Size / 2f));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, -Size / 2f, Size / 2f));
                    _newVertices.Add (new Vector3 (-Thickness / 2f, -Size / 2f, -Size / 2f));
                    break;
                case PrismaShapes.corner_slope:
                    _newVertices.Add (corners[0].localPosition);
                    _newVertices.Add (corners[1].localPosition);
                    _newVertices.Add (corners[2].localPosition);
                    _newVertices.Add (corners[0].localPosition);
                    _newVertices.Add (corners[3].localPosition);
                    _newVertices.Add (corners[2].localPosition);
                    break;
                case PrismaShapes.prisma:
                default:
                    _newVertices.Add (new Vector3 (0, Thickness / 2f, 0.5773F));
                    _newVertices.Add (new Vector3 (.5f, Thickness / 2f, -.2887f));
                    _newVertices.Add (new Vector3 (-.5f, Thickness / 2f, -.2887f));
                    _newVertices.Add (new Vector3 (0, -Thickness / 2f, 0.5773f));
                    _newVertices.Add (new Vector3 (.5f, -Thickness / 2f, -.2887f));
                    _newVertices.Add (new Vector3 (-.5f, -Thickness / 2f, -.2887f));
                    break;
            }

            Mesh mesh = new Mesh ();
            GetComponent<MeshFilter> ().mesh = mesh;
            mesh.vertices = _newVertices.ToArray ();
            mesh.triangles = _newTriangles;
            mesh.RecalculateNormals ();
            Rendered = true;
            MeshCollider meshc = GetComponent (typeof (MeshCollider)) as MeshCollider;
            meshc.sharedMesh = mesh; // Give it your mesh here.
        }
    }
}