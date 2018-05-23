﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief.PathFinding
{
    public class GuardMover : Pathfinding
    {

        public Guard owner;
        public Vector3 target;
        private bool newPath = true;
        private bool moving = false;

        public List<Vector3> MoverPath { get { return Path; } set { MoverPath = value; } }


        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }


        void Start()
        {
            owner = GetComponent<Guard>();
            Target = transform.position;
        }

       void Update()
       {
        //    if (Vector3.Distance(Target, transform.position) < 25f && !moving)
        //    {
        //        if (newPath)
        //        {
        //            StartCoroutine(NewPath());
        //        }
        //        moving = true;
        //    }
        //    else if (Vector3.Distance(Target, transform.position) < 2f)
        //    {
        //        //Stop!
        //        owner.Animation.SetBool("Moving", false);
        //    }
        //    else if (Vector3.Distance(Target, transform.position) < 35f && moving)
        //    {
        //        if (Path.Count > 0)
        //        {
                    
        //            if (Vector3.Distance(Target, Path[Path.Count - 1]) > 5f)
        //            {
        //                StartCoroutine(NewPath());
        //            }
        //        }
        //        else
        //        {
        //            if (newPath)
        //            {
        //                StartCoroutine(NewPath());
        //            }
        //        }
        //        //Move the ai towards the player
        //        MoveMethod();
        //    }
        //    else
        //    {
        //        moving = false;
        //    }

            if (Path.Count > 0)
            {

               owner.Animation.SetBool("Moving",true);
           }
            else
            {
                owner.Animation.SetBool("Moving", false);
            }
       }

        public IEnumerator NewPath()
        {
            newPath = false;
            target.y = 0f;
            FindPath(Target, owner.transform.position);
            yield return new WaitForSeconds(1F);
            newPath = true;
        }


        private void MoveMethod()
        {
            if (Path.Count > 0)
            {
                owner.Animation.SetBool("Moving", true);
                Vector3 direction = (Path[0] - transform.position).normalized;


                if (Vector3.Distance(owner.transform.position, transform.position) < 1F)
                {
                    Vector3 dir = (transform.position - owner.transform.position).normalized;
                    dir.Set(dir.x, 0, dir.z);
                    direction += 0.2F * dir;
                }
                

                direction.Normalize();


                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * 12F);
                if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
                {
                    Path.RemoveAt(0);
                }

                RaycastHit[] hit = Physics.RaycastAll(transform.position + (Vector3.up * 20F), Vector3.down, 100);
                float maxY = -Mathf.Infinity;
                foreach (RaycastHit h in hit)
                {
                    if (h.transform.tag == "Untagged")
                    {
                        if (maxY < h.point.y)
                        {
                            maxY = h.point.y;
                        }
                    }
                }
                if (maxY > -100)
                {
                    transform.position = new Vector3(transform.position.x, maxY + 1F, transform.position.z);
                }
            }
            else
            {
                owner.Animation.SetBool("Moving", false);
            }
        }
    }
}