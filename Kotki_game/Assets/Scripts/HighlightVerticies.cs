using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightVerticies : MonoBehaviour
{
    List<VertexPoint> verticies;
    Camera cam;

    [SerializeField]
    GameObject mobekToSpawn;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        RaycastHit hit;        
        if (Physics.Raycast(transform.position, mousePos, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Planet")
            {
                Debug.Log("skibidi almost toilet");
                verticies = hit.collider.gameObject.GetComponent<distributeverticies>().verticies;
                UntagAll();
                TagClosestVertex(hit.point);
            }
        }

    }

    void TagClosestVertex(Vector3 position)
    {
        VertexPoint closest = verticies[0];
        float smallestDist = Vector3.Distance(closest.position, position);
        foreach(var vertex in verticies)
        {
            float dist = Vector3.Distance(vertex.position, position);
            if (smallestDist> dist)
            {
                smallestDist = dist;
                closest = vertex;
            }
        }

        closest.highlighted = true;

        Debug.DrawLine(transform.position, closest.position, Color.red);
        Debug.Log("skibidi not toilet");
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject GO = Instantiate(mobekToSpawn, closest.position, transform.rotation);
            Debug.Log("skibidi!!!!!!!!!");
            Destroy(GO, 3f);
        }

        foreach (var neighbour in closest.neighbours)
        {
            Debug.DrawLine(transform.position, neighbour.position, Color.white);
            neighbour.highlighted = true;
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject GO = Instantiate(mobekToSpawn, neighbour.position, transform.rotation);
                Destroy(GO, 3f);
            }
        }
    }

    void UntagAll()
    {
        foreach (var vertex in verticies)
        {
            vertex.highlighted = false;
        }
    }
}