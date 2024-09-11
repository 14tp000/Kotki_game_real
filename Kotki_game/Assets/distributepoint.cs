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

    List<Vector3> verticies;

    List<Face> faces;

    private void Start()
    {
        verticies = new List<Vector3>();
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
        //basic d20
        //top
        Vector3 heightShift = new Vector3(0, ((0.5f + Mathf.Sqrt(5) / 10.0f)-(0.5f * (1.0f - 1.0f / Mathf.Sqrt(5))))/2, 0);
        float a = 0;
        for (int i = 0; i < 5; i++) {
            verticies.Add(1.0f/ Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a),0,Mathf.Cos(a))+heightShift);
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new Vector3(0, 0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0)+heightShift);

        faces.Add(new Face(verticies[0], verticies[1], verticies[5]));
        faces.Add(new Face(verticies[1], verticies[2], verticies[5]));
        faces.Add(new Face(verticies[2], verticies[3], verticies[5]));
        faces.Add(new Face(verticies[3], verticies[4], verticies[5]));
        faces.Add(new Face(verticies[4], verticies[0], verticies[5]));

        //bottom
        heightShift = -heightShift;
        a = Mathf.PI*2/10;
        for (int i = 0; i < 5; i++)
        {
            verticies.Add(1.0f / Mathf.Sqrt(5) * new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) + heightShift);
            a += Mathf.PI * 2 / 5;
        }
        verticies.Add(new Vector3(0, -0.5f * (1.0f - 1.0f / Mathf.Sqrt(5)), 0) + heightShift);

        faces.Add(new Face(verticies[6], verticies[7], verticies[11]));
        faces.Add(new Face(verticies[7], verticies[8], verticies[11]));
        faces.Add(new Face(verticies[8], verticies[9], verticies[11]));
        faces.Add(new Face(verticies[9], verticies[10], verticies[11]));
        faces.Add(new Face(verticies[10], verticies[6], verticies[11]));

        //middle
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
        for(int i = 0; i < verticies.Count; i++)
        {
            verticies[i] = (verticies[i] * scale/2) + transform.position;
        }

        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].vertA = faces[i].vertA * scale/2 + transform.position;
            faces[i].vertB = faces[i].vertB * scale/2 + transform.position;
            faces[i].vertC = faces[i].vertC * scale/2 + transform.position;
        }
    }

    void PlaceAllVertsOnSphere()
    {
        for (int i = 0; i < verticies.Count; i++)
        {
            verticies[i] = verticies[i].normalized;
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
        List<Vector3> newVerts = new List<Vector3>();

        foreach(var face in faces)
        {
            Vector3 midPointAB = midPoint(face.vertA, face.vertB);
            Vector3 midPointBC = midPoint(face.vertB, face.vertC);
            Vector3 midPointCA = midPoint(face.vertC, face.vertA);

            newVerts.Add(face.vertA);
            newVerts.Add(face.vertB);
            newVerts.Add(face.vertC);
            newVerts.Add(midPointAB);
            newVerts.Add(midPointBC);
            newVerts.Add(midPointCA);

            newFaces.Add(new Face(face.vertA,midPointCA,midPointAB));
            newFaces.Add(new Face(face.vertB,midPointBC,midPointAB));
            newFaces.Add(new Face(face.vertC,midPointBC,midPointCA));
            newFaces.Add(new Face(midPointAB,midPointBC,midPointCA));
        }

        verticies = newVerts;
        faces = newFaces;
        removeDuplicateVerts();
    }

    void removeDuplicateVerts()
    {
        HashSet<Vector3> hashVerts = new HashSet<Vector3>();

        foreach(var vert in verticies)
        {
            hashVerts.Add(vert);
        }

        verticies.Clear();

        foreach(var vert in hashVerts)
        {
            verticies.Add(vert);
        }
    }
    Vector3 midPoint(Vector3 v1, Vector3 v2)
    {
        return new Vector3((v1.x + v2.x) / 2, (v1.y + v2.y) / 2, (v1.z + v2.z) / 2);
    }

private void OnDrawGizmos()
{
    Gizmos.color = Color.red;

    // Sprawdź, czy lista verticies została zainicjalizowana
    if (verticies != null)
    {
        foreach (var pt in verticies)
        {
            Gizmos.DrawSphere(pt, gizmosRadius); // Rysujemy sfery bez dodatkowego skalowania
        }
    }

    Gizmos.color = Color.yellow;

    // Sprawdź, czy lista faces została zainicjalizowana
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
}
