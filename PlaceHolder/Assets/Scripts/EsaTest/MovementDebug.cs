using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TEST SCRIPT REMOVE WHEN NOT NEEDED.
public class MovementDebug : MonoBehaviour
{    
    public float movespeed = 5f;
    public float rotatespeed = 90f;
    
    // Update is called once per frame
    void Update ()
    {
        Move();
	}

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.back * movespeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * rotatespeed * Time.deltaTime);
        }

    }
}