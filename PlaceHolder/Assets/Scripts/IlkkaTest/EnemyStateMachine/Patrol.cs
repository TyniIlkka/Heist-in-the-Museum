using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI {
    public class Patrol : AIStateBase
    {
        private Path _path;
        private Direction _direction;
        private float _arriveDistance;

        public Waypoint CurrentWaypoint { get; private set; }

        public Patrol(Guard owner, Path path,
            Direction direction, float arriveDistance)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
            _path = path;
            _direction = direction;
            _arriveDistance = arriveDistance;
        }

        public override void StateActivated()
        {
            base.StateActivated();
            CurrentWaypoint = _path.GetClosestWaypoint(Owner.transform.position);
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {

                // 2. Are we close enough the current waypoint?
                //   2.1 If yes, get the next waypoint
                CurrentWaypoint = GetWaypoint();
                // 3. Move towards the current waypoint
                Owner.Move(Owner.transform.forward);
                // 4. Rotate towards the current waypoint
                Owner.Turn(CurrentWaypoint.Position);

            }
        }

        private Waypoint GetWaypoint()
        {
            Waypoint result = CurrentWaypoint;
            Vector3 toWaypointVector = CurrentWaypoint.Position - Owner.transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;
            if (toWaypointSqr <= sqrArriveDistance)
            {
                result = _path.GetNextWaypoint(CurrentWaypoint, ref _direction);
            }

            return result;
        }

        private bool ChangeState()
        {
            int soundLayer = Owner.SoundMask;

            Collider[] sounds = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, soundLayer);
            if (sounds.Length > 0)
            {
                foreach (Collider sound in sounds)
                {
                    DistractSound signal = sound.GetComponent<DistractSound>();

                    if (signal != null && signal.SoundOn == true)
                    {
                        return Owner.PerformTransition(AIStateType.PatrolMoveTo);  
                    }  
                }        
            }
            return false;
        }
    }
}
