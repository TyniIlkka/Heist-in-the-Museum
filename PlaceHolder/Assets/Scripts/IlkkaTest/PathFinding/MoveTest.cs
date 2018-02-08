﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectThief.PathFinding
{
    public class MoveTest : MonoBehaviour
    {

        private Transform myTransform;              // this transform
        private Vector3 destinationPosition;        // The destination Point
        private float destinationDistance;          // The distance between myTransform and destinationPosition

        public float moveSpeed;                         // The Speed the character will move



        void Start()
        {
            myTransform = transform;                            // sets myTransform to this GameObject.transform
            destinationPosition = myTransform.position;         // prevents myTransform reset
        }

        void Update()
        {

            // keep track of the distance between this gameObject and destinationPosition
            destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

            if (destinationDistance < .5f)
            {       // To prevent shakin behavior when near destination
                moveSpeed = 0;
            }
            else if (destinationDistance > .5f)
            {           // To Reset Speed to default
                moveSpeed = 3;
            }


            // Moves the Player if the Left Mouse Button was clicked
            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {

                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist))
                {
                    destinationPosition = ray.GetPoint(hitdist);
                    myTransform.position = destinationPosition;
                }
            }

            // Moves the player if the mouse button is hold down
            else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
            {

                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist))
                {
                    myTransform.position = ray.GetPoint(hitdist);
                }

            }
        }
    }
}
