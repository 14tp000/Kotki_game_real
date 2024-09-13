using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class distributeverticies : MonoBehaviour
{
    [SerializeField]
    float scale = 19;

    [SerializeField]
    int numSubdivisions = 2;//2 lub wiecej. kod sie nie zepsuje ale neighbours bedą popsute

    [SerializeField]
    float gizmosRadius = 0.1f;

    List<VertexPoint> verticies;

    List<Face> faces;

    private void Start()
    {
        verticies = new List<VertexPoint>();
        faces = new List<Face>();
        Debug.Log("skibidi toilett");

        IcoSphereDistribution();
        for (int i = 1; i < numSubdivisions; i++)
        {
            Subdivide();
        }
        PlaceAllVertsOnSphere();
        ScaleAndMove();
        FindNeighbours();
        PrintNeighbours();

    }

    void IcoSphereDistribution()
    {
        Vector3 heightShift = new Vector3(0, ((0.5f + Mathf.Sqrt(5) / 10.0f)-(0.5f * (1.0f - 1.0f / Mathf.Sqrt(5))))/2, 0);
        float a = 0;
        for (int i = 0; i < 5; i++) 
        {
            verticies.Add(new VertexPoint(1.0f/ Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a),0,Mathf.Cos(a))+heightShift));
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new VertexPoint(new Vector3(0, 0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0)+heightShift));

        faces.Add(new Face(verticies[0].position, verticies[1].position, verticies[5].position));
        faces.Add(new Face(verticies[1].position, verticies[2].position, verticies[5].position));
        faces.Add(new Face(verticies[2].position, verticies[3].position, verticies[5].position));
        faces.Add(new Face(verticies[3].position, verticies[4].position, verticies[5].position));
        faces.Add(new Face(verticies[4].position, verticies[0].position, verticies[5].position));

        heightShift = -heightShift;
        a = Mathf.PI * 2 / 10;
        for (int i = 0; i < 5; i++)
        {
            verticies.Add(new VertexPoint(1.0f / Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) + heightShift));
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new VertexPoint(new Vector3(0, -0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0) + heightShift));

        faces.Add(new Face(verticies[6].position, verticies[7].position, verticies[11].position));
        faces.Add(new Face(verticies[7].position, verticies[8].position, verticies[11].position));
        faces.Add(new Face(verticies[8].position, verticies[9].position, verticies[11].position));
        faces.Add(new Face(verticies[9].position, verticies[10].position, verticies[11].position));
        faces.Add(new Face(verticies[10].position, verticies[6].position, verticies[11].position));

        faces.Add(new Face(verticies[0].position, verticies[10].position, verticies[6].position));
        faces.Add(new Face(verticies[1].position, verticies[6].position, verticies[7].position));
        faces.Add(new Face(verticies[2].position, verticies[7].position, verticies[8].position));
        faces.Add(new Face(verticies[3].position, verticies[8].position, verticies[9].position));
        faces.Add(new Face(verticies[4].position, verticies[9].position, verticies[10].position));

        faces.Add(new Face(verticies[6].position, verticies[0].position, verticies[1].position));
        faces.Add(new Face(verticies[7].position, verticies[1].position, verticies[2].position));
        faces.Add(new Face(verticies[8].position, verticies[2].position, verticies[3].position));
        faces.Add(new Face(verticies[9].position, verticies[3].position, verticies[4].position));
        faces.Add(new Face(verticies[10].position, verticies[4].position, verticies[0].position));
    }

    void ScaleAndMove()
    {
        for (int i = 0; i < verticies.Count; i++)
        {
            verticies[i].position = (verticies[i].position * scale / 2) + transform.position;
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
            verticies[i].position = verticies[i].position.normalized;
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
        List<VertexPoint> newVerts = new List<VertexPoint>();

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
            hashVerts.Add(vert.position);
        }

        verticies.Clear();

        foreach (var vert in hashVerts)
        {
            verticies.Add(new VertexPoint(vert));
        }
    }

    Vector3 midPoint(Vector3 v1, Vector3 v2)
    {
        return new Vector3((v1.x + v2.x) / 2, (v1.y + v2.y) / 2, (v1.z + v2.z) / 2);
    }
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;


    if (verticies != null)
    {
        foreach (var pt in verticies)
        {
            Gizmos.DrawSphere(pt.position, gizmosRadius); 
        }
    }

    Gizmos.color = Color.yellow;


    if (faces != null)
    {
        foreach (var face in faces)
        {
            Gizmos.DrawLine(face.vertA, face.vertB);
            Gizmos.DrawLine(face.vertB, face.vertC);
            Gizmos.DrawLine(face.vertC, face.vertA);
        }
    }
}

private void FindNeighbours()
{
    foreach(var vert in verticies)
    {
        foreach(var face in faces)
        {
            if(vert.position==face.vertA){
                vert.SetNeighbour(face.vertB);
                vert.SetNeighbour(face.vertC);
            }
            else if(vert.position==face.vertB){
                vert.SetNeighbour(face.vertA);
                vert.SetNeighbour(face.vertC);
            }
            else if(vert.position==face.vertC){
                vert.SetNeighbour(face.vertA);
                vert.SetNeighbour(face.vertB);
            }
        }
    }
}
public void PrintNeighbours()
{
    foreach(var vert in verticies)
    {
        string neighboursInfo = "Neighbours: ";
        for (int i = 0; i < vert.neighbours.Length; i++)
        {
            neighboursInfo += $"[{i}] {vert.neighbours[i]}, ";
        }
        Debug.Log(vert.position + " amongus " + neighboursInfo);
        }
}

}
