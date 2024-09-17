using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    [SerializeField] float key_speed = 300;
    [SerializeField] float mouse_speed = 300;
    

    [SerializeField] float max_dist = 30;
    [SerializeField] float min_dist = 10;

    float dist_speed = 1;
    float distance = 20;

    float angleX = 0;
    float angleY = 0;

    void Update()
    {

    // movement
      float input_y = key_speed*Input.GetAxisRaw("Vertical")      - mouse_speed*(Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse Y");
      float input_x = -1*key_speed*Input.GetAxisRaw("Horizontal") + mouse_speed*(Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse X");

      angleY += input_x * Time.deltaTime * dist_speed;
      angleX += input_y * Time.deltaTime * dist_speed;

      transform.localEulerAngles = new Vector3(angleX, angleY, 0);
      transform.position = Vector3.zero - transform.forward * distance;
    
    // distance
      float input_dist = -1*Input.mouseScrollDelta.y;
      float dist_change = distance + input_dist;
      distance = distance >= min_dist ? (distance <= max_dist ? dist_change : max_dist) : min_dist;
      dist_speed = distance/20;
    }

}
