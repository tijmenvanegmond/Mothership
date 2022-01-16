// from http://docs.unity3d.com/ScriptReference/GL-wireframe.html
using UnityEngine;

public class ArrowRenderer : MonoBehaviour {
    public Camera RenderCamera;    
    public Color lineColor = new Color (0.0f, 1.0f, 1.0f);
    public float lineWidth = 3;
    public Material lineMaterial;   
    public int steps = 4;
    [Tooltip ("Steps must be divisible by lines")]
    public int lines = 4;
    private float stepSize;
    private Vector3[] vertices;
    private float radius = .5f;
    private float length = 1f;

    void Start () {
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        vertices = new Vector3[steps + 1];
    }

    // to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
    void DrawQuad (Vector3 p1, Vector3 p2) {
        float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
        Vector3 edge1 = -(RenderCamera.transform.position - (p2 + p1) / 2.0f); //vector from line center to camera
        Vector3 edge2 = p2 - p1; //vector from point to point
        Vector3 perpendicular = Vector3.Cross (edge1, edge2).normalized * thisWidth;

        GL.Vertex (p1 - perpendicular);
        GL.Vertex (p1 + perpendicular);
        GL.Vertex (p2 + perpendicular);
        GL.Vertex (p2 - perpendicular);
    }

    Vector3 to_world (Vector3 vec) {
        return gameObject.transform.TransformPoint (vec);
    }

        //create base verts
    public void UpdateArrow (float scalar, Color color ) {
        lineMaterial.color = color;
        length = Mathf.Min(1f, scalar * 2f);
        radius = Mathf.Min(.4f, scalar * 1.2f);
        float stepSize = (2f * Mathf.PI) / steps;
        for (int i = 0; i < steps; i++)
            vertices[i] = new Vector3 (Mathf.Cos (stepSize * i) * radius, Mathf.Sin (stepSize * i) * radius, 0);
        //add point vert
        vertices[steps] = Vector3.forward * length;
    }

    void OnRenderObject () {
        if (Camera.current.name == "UI Arrow Camera") {
            lineMaterial.SetPass (0);
            GL.Color (lineColor);
            GL.Begin (GL.QUADS);
            //draw base
            for (int i = 0; i < steps - 1; i++)
                DrawQuad (to_world (vertices[i]), to_world (vertices[i + 1]));
            DrawQuad (to_world (vertices[steps - 1]), to_world (vertices[0]));

            //draw lines to the point
            int bigStepSize = (steps - (steps % lines)) / lines;
            for (int i = 0; i < lines; i++)
                DrawQuad (to_world (vertices[i * bigStepSize]), to_world (vertices[steps]));

            GL.End ();
        }
    }
}
