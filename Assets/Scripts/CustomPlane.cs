using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]

public class CustomPlane : MonoBehaviour
{
    [Range(1,50)]
    public int cols = 1, rows = 1;
    [Range(0, 80f)]
    public int width = 1, height = 1;

#if UNITY_EDITOR
    void Awake()
    {
        MeshFilter meshFilter;
		meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) return;
        Mesh mesh = CreateCustomPlane(cols, rows, width, height);
        meshFilter.mesh = mesh;

    }
#endif

    private Mesh CreateCustomPlane(int cols, int rows, float width, float height)
    {
        if (cols <= 0 || rows <= 0 || width <= 0 || height <= 0) return new Mesh();
        Mesh mesh = new Mesh();
        mesh.name = "CustomPlane";
        mesh.vertices = CreateCustomVertices(cols, rows, width, height);
        mesh.triangles = CreateCustomTriangles(cols, rows);
        mesh.uv = CreateCustomUv(cols, rows);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    Vector3[] CreateCustomVertices(int cols, int rows, float width, float height)
    {
        var vertices = new Vector3[(cols + 1) * (rows + 1)];

        int length = vertices.Length;
        for (int i = 0; i < length; i++)
        {
            int x = i % (cols + 1);
            int z = i / (cols + 1);
            vertices[i] = new Vector3(
                width / 2 - (width / cols) * x,
                0,
                height / 2 - (height / rows) * z
            );
        }
        return vertices;
    }

    int[] CreateCustomTriangles(int cols, int rows)
    {
        var listTriangles = new List<int>();
        int verticesLength = (cols + 1) * (rows + 1);

        for (int i = 0; i < verticesLength - (cols + 1); i++)
        {
            if ((i % (cols + 1)) == cols) continue;

            listTriangles.Add(i);
            listTriangles.Add(i + cols + 1);
            listTriangles.Add(i + cols + 2);
            listTriangles.Add(i);
            listTriangles.Add(i + cols + 2);
            listTriangles.Add(i + 1);
        }

        return listTriangles.ToArray();
    }

    Vector2[] CreateCustomUv(int cols, int rows)
    {
        var uv = new Vector2[(cols + 1) * (rows + 1)];
        int length = uv.Length;

        for (int i = 0; i < length; i++)
        {
            int u = i % (cols + 1);
            int v = i / (cols + 1);
            uv[i] = new Vector2(
                1.0f - (1.0f / cols) * u,
                1.0f - (1.0f / rows) * v
            );
        }

        return uv;
    }
}