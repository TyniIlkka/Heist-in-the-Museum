using System.Collections;
using System.Collections.Generic;
using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        public Node CurrentWaypoint { get; private set; }

        public PatrolMoveTo(Guard owner)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
        }

        public override void StateActivated()
        {
            base.StateActivated();
            //CurrentWaypoint = _path.GetClosestWaypoint(Owner.transform.position);
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                // 2. Are we close enough the current waypoint?
                //   2.1 If yes, get the next waypoint
                //CurrentWaypoint = GetWaypoint();
                // 3. Move towards the current waypoint
                Owner.Move(Owner.transform.forward);
                // 4. Rotate towards the current waypoint
                //Owner.Turn();
            }
        }

        private Node GetDistractionNode()
        {
            return null;
        }

        private bool ChangeState()
        {
            //int mask = LayerMask.GetMask( "Player" );
            int soundLayer = LayerMask.NameToLayer("SoundOutput");

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, soundLayer);
            if (players.Length > 0)
            {
                
            }
            return false;
        }

    }
}
