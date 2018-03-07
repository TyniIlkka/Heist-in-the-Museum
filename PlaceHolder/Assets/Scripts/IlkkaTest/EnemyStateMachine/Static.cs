﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI
{
    public class Static : AIStateBase
    {
        private MyDirections m_eDirection;

        public Static(Guard owner, MyDirections currentDirection)
            : base()
        {
            State = AIStateType.Static;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
            m_eDirection = currentDirection;

        }

        public override void StateActivated()
        {
            base.StateActivated();
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                // 2. Stay on place.
                switch (m_eDirection)
                {
                    case MyDirections.North:
                        Owner.transform.forward = new Vector3(0f, 0f, 1f);
                        Owner.Direction = new Vector3(0f, 0f, 1f);
                        break;
                    case MyDirections.East:
                        Owner.transform.forward = new Vector3(1f, 0f, 0f);
                        Owner.Direction = new Vector3(1f, 0f, 0f);
                        break;
                    case MyDirections.South:
                        Owner.transform.forward = new Vector3(0f, 0f, -1f);
                        Owner.Direction = new Vector3(0f, 0f, -1f);
                        break;
                    case MyDirections.West:
                        Owner.transform.forward = new Vector3(-1f, 0f, 0f);
                        Owner.Direction = new Vector3(-1f, 0f, 0f);
                        break;
                }
            }
        }

        private bool ChangeState()
        {
            int soundLayer = Owner.LightMask;

            Collider[] lights = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, soundLayer);
            if (lights.Length > 0)
            {
                foreach (Collider light in lights)
                {
                    DistractLight lightSignal = light.GetComponent<DistractLight>();
                    if (lightSignal.LightOn == true && lightSignal != null)
                    {
                        return Owner.PerformTransition(AIStateType.StaticTurnTo);
                    }
                }
            }
            return false;
        }

    }
}