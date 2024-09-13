using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexPoint
{
    public Vector3 position;
      public Vector3[] neighbours = new Vector3[6];
    public VertexPoint(Vector3 position)
    {
        this.position = position;

    }
    public void SetNeighbour(Vector3 neighbour)
    {
        for (int i = 0; i < 6; i++)
        {
            if (neighbours[i] == neighbour)
            {
                break;
            }

            if (neighbours[i] == Vector3.zero)
            {
                neighbours[i] = neighbour;
                break;
            }
        }
}


}