using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public Vector3 vertA;
    public Vector3 vertB;
    public Vector3 vertC;

    
    public Face(Vector3 vertA, Vector3 vertB, Vector3 vertC)
    {
        this.vertA = vertA;
        this.vertB = vertB;
        this.vertC = vertC;

    }
}