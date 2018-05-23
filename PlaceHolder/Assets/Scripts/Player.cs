﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief
{
    public class Player : CharacterBase
    {
        private Animator m_aPlayerAnimator;
        public List<Vector3> Path = new List<Vector3>();

        public float speed;

        public float walkRange;
        public float sneakRange;

        public Animator AnimationPlayer { get { return m_aPlayerAnimator; } }
        

        public float DetectRange
        {
            get
            {
                if (Speed > 2f)
                {
                    return walkRange;
                }
                else if ( 0f < Speed && Speed < 2f)
                {
                    return sneakRange;
                }
                else
                {
                    return 0f;
                }
            }
            set
            {

            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
            
        }

        public override void Init()
        {
            Player player = GetComponent<Player>();

            m_aPlayerAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// Sets moving animation if player have path to walk
        /// </summary>
        /// <param name="path">Current path progress</param>
        public void MoveAnimation(List<Vector3> path)
        {
            if (path.Count > 0)
            {
                m_aPlayerAnimator.SetBool("Moving", true);
                //Debug.Log("Moving");
            }
            else
            {
                m_aPlayerAnimator.SetBool("Moving", false);
                //Debug.Log("Idle");
            }
        }

        public override void Move(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public override void Turn(Vector3 amount)
        {
            throw new System.NotImplementedException();
        }
    }
}