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

    public List<VertexPoint> verticies;

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
        Debug.Log("numverts = " + verticies.Count);
        FindNeighbours();
        Debug.Log("numverts2 = " + verticies.Count);
        ScaleAndMove();
        //PrintNeighbours();

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

        faces.Add(new Face(verticies[0], verticies[1], verticies[5]));
        faces.Add(new Face(verticies[1], verticies[2], verticies[5]));
        faces.Add(new Face(verticies[2], verticies[3], verticies[5]));
        faces.Add(new Face(verticies[3], verticies[4], verticies[5]));
        faces.Add(new Face(verticies[4], verticies[0], verticies[5]));

        heightShift = -heightShift;
        a = Mathf.PI * 2 / 10;
        for (int i = 0; i < 5; i++)
        {
            verticies.Add(new VertexPoint(1.0f / Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) + heightShift));
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new VertexPoint(new Vector3(0, -0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0) + heightShift));

        faces.Add(new Face(verticies[6], verticies[7], verticies[11]));
        faces.Add(new Face(verticies[7], verticies[8], verticies[11]));
        faces.Add(new Face(verticies[8], verticies[9], verticies[11]));
        faces.Add(new Face(verticies[9], verticies[10], verticies[11]));
        faces.Add(new Face(verticies[10], verticies[6], verticies[11]));

        faces.Add(new Face(verticies[0], verticies[10], verticies[6]));
        faces.Add(new Face(verticies[1], verticies[6], verticies[7]));
        faces.Add(new Face(verticies[2], verticies[7], verticies[8]));
        faces.Add(new Face(verticies[3], verticies[8], verticies[9]));
        faces.Add(new Face(verticies[4], verticies[9], verticies[10]));

        faces.Add(new Face(verticies[6], verticies[0], verticies[1]));
        faces.Add(new Face(verticies[7], verticies[1], verticies[2]));
        faces.Add(new Face(verticies[8], verticies[2], verticies[3]));
        faces.Add(new Face(verticies[9], verticies[3], verticies[4]));
        faces.Add(new Face(verticies[10], verticies[4], verticies[0]));
    }

    void ScaleAndMove()
    {
        for (int i = 0; i < verticies.Count; i++)
        {
            verticies[i].position = (verticies[i].position * scale / 2) + transform.position;
        }

        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].vertA = new VertexPoint(faces[i].vertA.position * scale / 2 + transform.position);
            faces[i].vertB = new VertexPoint(faces[i].vertB.position * scale / 2 + transform.position);
            faces[i].vertC = new VertexPoint(faces[i].vertC.position * scale / 2 + transform.position);
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
            faces[i].vertA = new VertexPoint(faces[i].vertA.position.normalized);
            faces[i].vertB = new VertexPoint(faces[i].vertB.position.normalized);
            faces[i].vertC = new VertexPoint(faces[i].vertC.position.normalized);
        }
    }

    void Subdivide()
    {
        List<Face> newFaces = new List<Face>();
        List<VertexPoint> newVerts = new List<VertexPoint>();

        foreach (var face in faces)
        {
            Vector3 midPointAB = midPoint(face.vertA.position, face.vertB.position);
            Vector3 midPointBC = midPoint(face.vertB.position, face.vertC.position);
            Vector3 midPointCA = midPoint(face.vertC.position, face.vertA.position);

            newVerts.Add(face.vertA);
            newVerts.Add(face.vertB);
            newVerts.Add(face.vertC);
            newVerts.Add(new VertexPoint(midPointAB));
            newVerts.Add(new VertexPoint(midPointBC));
            newVerts.Add(new VertexPoint(midPointCA));

            newFaces.Add(new Face(face.vertA, new VertexPoint(midPointCA), new VertexPoint(midPointAB)));
            newFaces.Add(new Face(face.vertB, new VertexPoint(midPointBC), new VertexPoint(midPointAB)));
            newFaces.Add(new Face(face.vertC, new VertexPoint(midPointBC), new VertexPoint(midPointCA)));
            newFaces.Add(new Face(new VertexPoint(midPointAB), new VertexPoint(midPointBC), new VertexPoint(midPointCA)));
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
    //private void FindNeighbours()
    //{
    //    foreach(var vert in verticies)
    //    {
    //        foreach(var face in faces)
    //        {
    //            if(vert.position==face.vertA.position)
    //                {
    //                vert.SetNeighbour(face.vertB);
    //                vert.SetNeighbour(face.vertC);
    //            }
    //            else if(vert.position==face.vertB.position){
    //                vert.SetNeighbour(face.vertA);
    //                vert.SetNeighbour(face.vertC);
    //            }
    //            else if(vert.position==face.vertC.position){
    //                vert.SetNeighbour(face.vertA);
    //                vert.SetNeighbour(face.vertB);
    //            }
    //        }
    //    }
    //}

    void FindNeighbours()
    {
        for(int i = 0;i<verticies.Count;i++)
        {
            VertexPoint tmp = FindClosestInList(verticies[i], verticies);
            verticies[i].neighbours.Add(tmp);
            for (int j = 1; j < 6; j++)
            {
                verticies[i].neighbours.Add(FindClosestInList(verticies[i], verticies, verticies[i].neighbours));
            }
        }
    }

    VertexPoint FindClosestInList(VertexPoint point,List<VertexPoint> list,List<VertexPoint> exclude)
    {
        VertexPoint closest = list[0];
        float smallestDist = Vector3.Distance(point.position, closest.position);

        foreach(var vertex in list)
        {
            if (vertex != point && !exclude.Contains(vertex))
            {
                float dist = Vector3.Distance(point.position, vertex.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    closest = vertex;
                }
            }
        }
        return closest;
    }

    VertexPoint FindClosestInList(VertexPoint point, List<VertexPoint> list)
    {

        VertexPoint closest = list[0];
        float smallestDist = Vector3.Distance(point.position, closest.position);

        foreach (var vertex in list)
        {
            if (vertex != point)
            {
                float dist = Vector3.Distance(point.position, vertex.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    closest = vertex;
                }
            }
        }
        return closest;
    }

    public void PrintNeighbours()
    {
        foreach(var vert in verticies)
        {
            string neighboursInfo = "Neighbours: ";
            for (int i = 0; i < vert.neighbours.Count; i++)
            {
                neighboursInfo += $"[{i}] {vert.neighbours[i]}, ";
            }
            Debug.Log(vert.position + " amongus " + neighboursInfo);
            }
    }
    private void OnDrawGizmos()
    {



        if (verticies != null)
        {
            foreach (var pt in verticies)
            {
                if (pt.highlighted)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pt.position, gizmosRadius);
                }
                else
                {
                    Gizmos.color = Color.red;
                }

            }
        }

        Gizmos.color = Color.yellow;


        if (faces != null)
        {
            foreach (var face in faces)
            {
                Gizmos.DrawLine(face.vertA.position, face.vertB.position);
                Gizmos.DrawLine(face.vertB.position, face.vertC.position);
                Gizmos.DrawLine(face.vertC.position, face.vertA.position);
            }
        }
    }

}
