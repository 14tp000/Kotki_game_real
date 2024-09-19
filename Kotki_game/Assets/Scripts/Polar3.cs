using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polar3 : MonoBehaviour
{
    public float phi;
    public float theta;
    public float r;
    public Polar3(Vector3 kart)
    {
        float r = Mathf.Sqrt(kart.x * kart.x + kart.y * kart.y + kart.z * kart.z); // ur mom
        float phi = Mathf.Atan2(kart.y, kart.x); // w osi XZ
        float theta = Mathf.Acos(kart.z / r); // w psi Z

        this.r = r;
        this.phi = phi;
        this.theta = theta;
    }
}
