using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    [SerializeField] float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      // rotation
      transform.LookAt(Vector3.zero); 
      // movement
      float input_y = Input.GetAxis("Vertical")   + (Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse Y");
      float input_x = Input.GetAxis("Horizontal") - (Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse X");
      Vector3 velocity = transform.up*input_y + transform.right*input_x;
      velocity.Normalize();

      transform.position += velocity * speed * Time.deltaTime;
    }
}
