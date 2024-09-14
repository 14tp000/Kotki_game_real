using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face 
{
    public VertexPoint vertA;
    public VertexPoint vertB;
    public VertexPoint vertC;
    public Face(VertexPoint vertA, VertexPoint vertB, VertexPoint vertC)
    {
        this.vertA = vertA;
        this.vertB = vertB;
        this.vertC = vertC;
    }
}