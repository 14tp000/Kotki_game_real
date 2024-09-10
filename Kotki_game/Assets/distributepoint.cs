using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distributeverticies : MonoBehaviour
{
    [SerializeField]
    float scale = 10;

    [SerializeField]
    int numSubdivisions = 1;

    [SerializeField]
    float gizmosRadius = 0.1f;

    // Zmieniamy typ z List<Vector3> na List<Vertex>
    List<VertexPoint> verticies;
    List<Face> faces;

    private void Start()
    {
        verticies = new List<VertexPoint>();
        faces = new List<Face>();

        IcoSphereDistribution();
        for (int i = 1; i < numSubdivisions; i++)
        {
            Subdivide();
        }
        PlaceAllVertsOnSphere();
        ScaleAndMove();

        Debug.Log(verticies.Count);
    }

    void IcoSphereDistribution()
    {
        // basic d20
        // top
        Vector3 heightShift = new Vector3(0, ((0.5f + Mathf.Sqrt(5) / 10.0f) - (0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)))) / 2, 0);
        float a = 0;
        for (int i = 0; i < 5; i++)
        {
            Vector3 vert = 1.0f / Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) + heightShift;
            verticies.Add(new VertexPoint(vert));  // Dodajemy instancję Vertex
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new VertexPoint(new Vector3(0, 0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0) + heightShift)); // Dodajemy instancję Vertex

        faces.Add(new Face(verticies[0].vertA, verticies[1].vertA, verticies[5].vertA));
        faces.Add(new Face(verticies[1].vertA, verticies[2].vertA, verticies[5].vertA));
        faces.Add(new Face(verticies[2].vertA, verticies[3].vertA, verticies[5].vertA));
        faces.Add(new Face(verticies[3].vertA, verticies[4].vertA, verticies[5].vertA));
        faces.Add(new Face(verticies[4].vertA, verticies[0].vertA, verticies[5].vertA));

        // bottom
        heightShift = -heightShift;
        a = Mathf.PI * 2 / 10;
        for (int i = 0; i < 5; i++)
        {
            Vector3 vert = 1.0f / Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) + heightShift;
            verticies.Add(new VertexPoint(vert));  // Dodajemy instancję Vertex
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new VertexPoint(new Vector3(0, -0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0) + heightShift)); // Dodajemy instancję Vertex

        faces.Add(new Face(verticies[6].vertA, verticies[7].vertA, verticies[11].vertA));
        faces.Add(new Face(verticies[7].vertA, verticies[8].vertA, verticies[11].vertA));
        faces.Add(new Face(verticies[8].vertA, verticies[9].vertA, verticies[11].vertA));
        faces.Add(new Face(verticies[9].vertA, verticies[10].vertA, verticies[11].vertA));
        faces.Add(new Face(verticies[10].vertA, verticies[6].vertA, verticies[11].vertA));

        // middle
        faces.Add(new Face(verticies[0].vertA, verticies[10].vertA, verticies[6].vertA));
        faces.Add(new Face(verticies[1].vertA, verticies[6].vertA, verticies[7].vertA));
        faces.Add(new Face(verticies[2].vertA, verticies[7].vertA, verticies[8].vertA));
        faces.Add(new Face(verticies[3].vertA, verticies[8].vertA, verticies[9].vertA));
        faces.Add(new Face(verticies[4].vertA, verticies[9].vertA, verticies[10].vertA));

        faces.Add(new Face(verticies[6].vertA, verticies[0].vertA, verticies[1].vertA));
        faces.Add(new Face(verticies[7].vertA, verticies[1].vertA, verticies[2].vertA));
        faces.Add(new Face(verticies[8].vertA, verticies[2].vertA, verticies[3].vertA));
        faces.Add(new Face(verticies[9].vertA, verticies[3].vertA, verticies[4].vertA));
        faces.Add(new Face(verticies[10].vertA, verticies[4].vertA, verticies[0].vertA));
    }

    void ScaleAndMove()
    {
        for (int i = 0; i < verticies.Count; i++)
        {
            verticies[i].vertA = (verticies[i].vertA * scale / 2) + transform.position;
        }

        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].vertA = faces[i].vertA * scale / 2 + transform.position;
            faces[i].vertB = faces[i].vertB * scale / 2 + transform.position;
            faces[i].vertC = faces[i].vertC * scale / 2 + transform.position;
        }
    }

    void PlaceAllVertsOnSphere()
    {
        for (int i = 0; i < verticies.Count; i++)
        {
            verticies[i].vertA = verticies[i].vertA.normalized;
        }

        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].vertA = faces[i].vertA.normalized;
            faces[i].vertB = faces[i].vertB.normalized;
            faces[i].vertC = faces[i].vertC.normalized;
        }
    }

    void Subdivide()
    {
        List<Face> newFaces = new List<Face>();
        List<Vertex> newVerts = new List<Vertex>();

        foreach (var face in faces)
        {
            Vector3 midPointAB = midPoint(face.vertA, face.vertB);
            Vector3 midPointBC = midPoint(face.vertB, face.vertC);
            Vector3 midPointCA = midPoint(face.vertC, face.vertA);

            newVerts.Add(new VertexPoint(face.vertA));
            newVerts.Add(new VertexPoint(face.vertB));
            newVerts.Add(new VertexPoint(face.vertC));
            newVerts.Add(new VertexPoint(midPointAB));
            newVerts.Add(new VertexPoint(midPointBC));
            newVerts.Add(new VertexPoint(midPointCA));

            newFaces.Add(new Face(face.vertA, midPointCA, midPointAB));
            newFaces.Add(new Face(face.vertB, midPointBC, midPointAB));
            newFaces.Add(new Face(face.vertC, midPointBC, midPointCA));
            newFaces.Add(new Face(midPointAB, midPointBC, midPointCA));
        }

        verticies = newVerts;
        faces = newFaces;
        removeDuplicateVerts();
    }

    void removeDuplicateVerts()
    {
        HashSet<Vector3> hashVerts = new HashSet<Vector3>();

        foreach (var vert in verticies)
        {
            hashVerts.Add(vert.vertA);
        }

        verticies.Clear();

        foreach (var vert in hashVerts)
        {
            verticies.Add(new Vertex(vert));
        }
    }

    Vector3 midPoint(Vector3 v1, Vector3 v2)
    {
        return new Vector3((v1.x + v2.x) / 2, (v1.y + v2.y) / 2, (v1.z + v2.z) / 2);
    }

private void OnDrawGizmos()
{
if (verticies != null)
{
    Gizmos.color = Color.red;
    foreach (var pt in verticies)
    {
        if (pt != null)
        {
            Gizmos.DrawSphere(pt.vertA, gizmosRadius);
        }
        else
        {
            Debug.Log("Null vertex found!");
        }
    }
}
else
{
    Debug.Log("Vertices list is null!");
}
}

}
