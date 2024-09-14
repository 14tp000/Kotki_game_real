using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexPoint
{
    public Vector3 position;
    public List<VertexPoint> neighbours = new List<VertexPoint>();
    public bool highlighted;
    public VertexPoint(Vector3 position)
    {
        this.position = position;
    }
}