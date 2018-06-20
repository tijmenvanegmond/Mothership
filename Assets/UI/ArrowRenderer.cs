// from http://docs.unity3d.com/ScriptReference/GL-wireframe.html
using UnityEngine;
using System.Collections;

public class ArrowRenderer : MonoBehaviour
{
	public Camera RenderCamera;
	public Color lineColor = new Color(0.0f, 1.0f, 1.0f);
	public float lineWidth = 3;
	public int size = 0;
	public Material lineMaterial;
	public float radius = .5f;
	public float height = 1f;
	//hardcoded 16 verts in the ring
	float stepSize = (2f * Mathf.PI) / 16f;
	Vector3[] vertices = new Vector3[17];

	void Start()
	{
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
	}

	// to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
	void DrawQuad(Vector3 p1, Vector3 p2)
	{
		float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
		Vector3 edge1 = (RenderCamera.transform.position - (p2 + p1) / 2.0f)*-1;    //vector from line center to camera
		Vector3 edge2 = p2 - p1;    //vector from point to point
		Vector3 perpendicular = Vector3.Cross(edge1, edge2).normalized * thisWidth;

		GL.Vertex(p1 - perpendicular);
		GL.Vertex(p1 + perpendicular);
		GL.Vertex(p2 + perpendicular);
		GL.Vertex(p2 - perpendicular);
	}

	Vector3 to_world(Vector3 vec)
	{
		return gameObject.transform.TransformPoint(vec);
	}

	/*
	████████       █  █  █▀▀▄  █▀▀▄  ▄▀▀▄  ▀▀█▀▀  █▀▀▀
	████████       █  █  █▀▀   █  █  █■■█    █    █■■
	████████       ▀▄▄▀  █     █▄▄▀  █  █    █    █▄▄▄
	*/

	void Update()
	{
		for (int i = 0; i < 16; i++)
		{
			vertices[i] = new Vector3(Mathf.Cos(stepSize * i) * radius, Mathf.Sin(stepSize * i) * radius, 0);
		}

		vertices[16] = Vector3.forward * height;
	}

	void OnRenderObject()
	{
		if (Camera.current.name == "UI Arrow Camera")
		{
			lineMaterial.SetPass(0);
			GL.Color(lineColor);
			GL.Begin(GL.QUADS);
			for (int i = 0; i < 15; i++)
			{
				DrawQuad(to_world(vertices[i]), to_world(vertices[i + 1]));
			}
			DrawQuad(to_world(vertices[15]), to_world(vertices[0]));
			DrawQuad(to_world(vertices[0]), to_world(vertices[16]));
			DrawQuad(to_world(vertices[4]), to_world(vertices[16]));
			DrawQuad(to_world(vertices[8]), to_world(vertices[16]));
			DrawQuad(to_world(vertices[12]), to_world(vertices[16]));

			GL.End();
		}
	}
}
