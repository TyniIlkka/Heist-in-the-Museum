using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief.AI
{
    public class Static : AIStateBase
    {

        public Static(Guard owner)
            : base()
        {
            State = AIStateType.Static;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);

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
                // 2. Are we close enough the current waypoint?
                //   2.1 If yes, get the next waypoint
                CurrentWaypoint = GetWaypoint();
                // 3. Rotate towards normal.
                Owner.Turn(target.Position);
            }
        }

        private bool ChangeState()
        {
            //int mask = LayerMask.GetMask( "Player" );
            int soundLayer = LayerMask.NameToLayer("SoundOutput");

            Collider[] lights = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, soundLayer);
            if (lights.Length > 0)
            {
                if (lights != null)
                {
                    Owner.Target = player;
                    float sqrDistanceToPlayer = Owner.ToTargetVector.Value.sqrMagnitude;
                    if (sqrDistanceToPlayer <
                         Owner.DetectEnemyDistance * Owner.DetectEnemyDistance)
                    {
                        return Owner.PerformTransition(AIStateType.StaticTurnTo);
                    }

                    Owner.Target = null;
                }
            }
            return false;
        }

    }
}
