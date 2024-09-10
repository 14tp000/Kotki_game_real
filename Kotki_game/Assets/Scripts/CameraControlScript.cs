using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float distance = 10;

    float angleX = 0;
    float angleY = 0;

    void Update()
    {
        // rotation

        // movement

        //Vector3 velocity = transform.up*input_y + transform.right*input_x;
        //velocity.Normalize();


        float input_y = Input.GetAxisRaw("Vertical") + (Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse Y");
        float input_x = -Input.GetAxisRaw("Horizontal") - (Input.GetMouseButton(2) ? 1 : 0) * Input.GetAxis("Mouse X");

        Debug.Log(input_x);
        angleY += input_x * Time.deltaTime* speed;
        angleX += input_y * Time.deltaTime*speed;

        transform.localEulerAngles = new Vector3(angleX, angleY,0);

        transform.position = Vector3.zero - transform.forward * distance;
    }

}
