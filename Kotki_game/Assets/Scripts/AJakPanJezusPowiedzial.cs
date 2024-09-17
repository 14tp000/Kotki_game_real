using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartNaBieg : MonoBehaviour
{
  static (float phi, float theta, float r) TakJakPanJezusPowiedzial(float x, float y, float z)
  {
    float r = Math.Sqrt(x*x + y*y + z*z); // ur mom
    float phi = Math.Atan2(y, x); // w osi XZ
    float theta = Math.Acos(z / r); // w psi Z

    return (phi, theta, r);
  }
}
