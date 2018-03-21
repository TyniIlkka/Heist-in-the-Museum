﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI {
    public class Patrol : AIStateBase
    {
        private PathPoints _path;
        private Direction _direction;
        private float _arriveDistance;


        public Patrol(Guard owner, List<PathPoints> path,
            Direction direction, float arriveDistance, int currentPathNumber)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
            _path = path[currentPathNumber];
            _direction = direction;
            _arriveDistance = arriveDistance;
        }

        public override void StateActivated()
        {
            base.StateActivated();
            Owner.CurrentWaypoint = _path.GetClosestWaypoint(Owner.transform.position);
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {

                // 2. Are we close enough the current waypoint?
                //   2.1 If yes, get the next waypoint
                Owner.CurrentWaypoint = GetWaypoint();
                // 3. Move towards the current waypoint
                Owner.Move(Owner.transform.forward);
                // 4. Rotate towards the current waypoint
                Owner.Turn(Owner.CurrentWaypoint.Position);

            }
        }

        private Waypoint GetWaypoint()
        {
            Waypoint result = Owner.CurrentWaypoint;
            Vector3 toWaypointVector = Owner.CurrentWaypoint.Position - Owner.transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;
            if (toWaypointSqr <= sqrArriveDistance)
            {
                result = _path.GetNextWaypoint(Owner.CurrentWaypoint, ref _direction);
            }

            return result;
        }

        private bool ChangeState()
        {
            if (Owner.Distracted)
            {
                Debug.Log("Hämätty äänellä!");
                bool result = Owner.PerformTransition(AIStateType.PatrolMoveTo);
                return result;
            }
            return false;
        }
    }
}
