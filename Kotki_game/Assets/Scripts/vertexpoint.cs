using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexPoint
{
    public Vector3 position;
    public List<VertexPoint> neighbours = new List<VertexPoint>();
    public bool highlighted;
    int iterator = 0;
    public VertexPoint(Vector3 position)
    {
        this.position = position;
    }
    public void SetNeighbour(VertexPoint neighbour)
    {
        neighbours.Add(neighbour);
        
        //for (int i = 0; i < 6; i++)
        //{
        //    //if (neighbours[i] == neighbour)
        //    //{
        //    //    break;
        //    //}
        //    neighbours[i] = neighbour;
        //    //if (neighbours[i].position == Vector3.zero)
        //    //{
        //    //    neighbours[i] = neighbour;
        //    //    break;
        //    //}
        //}
    }
}