using System.Linq;
using UnityEngine;
using Artificialintelligence;
using System.Collections.Generic;

public class Triangle : IntelligentEntity
{
    protected override void CreateShape()
    {
        // Create Vector2 vertices
        var vertices2D = new Vector2[] {
            new Vector2(-1,0) * size,
            new Vector2(1,0) * size,
            new Vector2(0,3) * size,
        };

        var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        // Use the triangulator to get indices for creating triangles
        // Note the triangulator is a third-party script
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        //var colors = Enumerable.Range(0, vertices3D.Length)
        //    .Select(i => Random.ColorHSV())
        //    .ToArray();

        // Create the mesh
        var mesh = new Mesh
        {
            vertices = vertices3D,
            triangles = indices,
            colors = new Color[] { Color.green, Color.green, Color.green }
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;

        var collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.points = vertices2D;
    }

    protected override void ChangeColor(Color color)
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        mesh.colors = new Color[] { color, color, color };
    }

    protected override void ProcessOutput(float[] outputs)
    {
        Movement movement = GetComponent<Movement>();
        movement.ApplyTorque(outputs[0]);
    }

    protected override float[] GenerateInput()
    {
        List<float> inputs = new List<float>();

        inputs.AddRange(Raycast(90));
        inputs.AddRange(RaycastReflect(135, 180));

        return inputs.ToArray();
    }
}