using System.Collections;
using System.Collections.Generic;
using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {

        private List<Node> _path;
        private Direction _direction;
        private float _arriveDistance;

        public Waypoint CurrentWaypoint { get; private set; }

        public Patrol(Guard owner, List<Node> path,
             float arriveDistance)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
            _path = path;
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

        private List<Node> GetWaypoint()
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
            //int mask = LayerMask.GetMask( "Player" );
            int soundLayer = LayerMask.NameToLayer("SoundOutput");
            int mask = Flags.CreateMask(soundLayer);

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, mask);
            if (players.Length > 0)
            {
                PlayerUnit player =
                    players[0].gameObject.GetComponentInHierarchy<PlayerUnit>();

                if (player != null)
                {
                    Owner.Target = player;
                    float sqrDistanceToPlayer = Owner.ToTargetVector.Value.sqrMagnitude;
                    if (sqrDistanceToPlayer <
                         Owner.DetectEnemyDistance * Owner.DetectEnemyDistance)
                    {
                        return Owner.PerformTransition(AIStateType.FollowTarget);
                    }

                    Owner.Target = null;
                }
            }
            return false;
        }

    }
}
