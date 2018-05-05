using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI {
    public class Patrol : AIStateBase
    {
        private PathPoints _path;
        private Direction _direction;
        private float _arriveDistance;


        public Patrol(Guard owner, PathPoints path,
            Direction direction, float arriveDistance)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
            AddTransition(AIStateType.Static);
            _path = path;
            _direction = direction;
            _arriveDistance = arriveDistance;
        }

        public override void StateActivated()
        {
            base.StateActivated();

            Owner.CurrentWaypoint = GetWaypoint();
            
        }
        void OnEnable()
        {
            Debug.Log("Enablee?");
            Owner.CurrentWaypoint = _path.GetClosestWaypoint(Owner.transform.position);
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.
            //if (Owner.CurrentWaypoint == null && _path != null)
            //{
            //    StateActivated();
            //}


            if (!ChangeState())
            {

                // 2. Are we close enough the current waypoint?
                //   2.1 If yes, get the next waypoint
                if (GameManager.instance.canMove)
                {
                    Owner.CurrentWaypoint = GetWaypoint();
                    // 3. Move towards the current waypoint
                    Owner.Move(Owner.transform.forward);
                    // 4. Rotate towards the current waypoint
                    Owner.Turn(Owner.CurrentWaypoint.Position);
                }
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
            if (Owner.DistractedSound)
            {
                Debug.Log("Hämätty äänellä!");
                bool result = Owner.PerformTransition(AIStateType.PatrolMoveTo);
                return result;
            }
            if (!Owner.Moving)
            {
                bool result = Owner.PerformTransition(AIStateType.Static);
            }
            return false;
        }
    }
}
