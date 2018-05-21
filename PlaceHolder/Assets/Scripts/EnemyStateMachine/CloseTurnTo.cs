using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectThief.PathFinding;

namespace ProjectThief.AI
{
    public class CloseTurnTo : AIStateBase
    {
        public float ready;

        public CloseTurnTo(Guard owner)
            : base()
        {
            State = AIStateType.CloseTurnTo;
            Owner = owner;

            AddTransition(AIStateType.Patrol);
            AddTransition(AIStateType.PatrolMoveBack);
            AddTransition(AIStateType.PatrolMoveTo);
            AddTransition(AIStateType.PatrolStayAtTarget);
            AddTransition(AIStateType.Static);
            AddTransition(AIStateType.StaticTurnTo);

        }

        public override void StateActivated()
        {
            base.StateActivated();
            Mover = Owner.GetComponent<GuardMover>();

            ready = 0f; //Reset Timer.
        }

        public override void StateDeactivating()
        {
            base.StateDeactivating();
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.
            ready++;
            if (!ChangeState())
            {
                //2. Turn to Target
                Owner.Turn(Owner.Thief.transform.position);


            }
        }

        /// <summary>
        /// Change state, this state change when object is close enough current waypoint.
        /// </summary>
        /// <returns>Bool result</returns>
        private bool ChangeState()
        {
            if (ready >= 3)
            {
                bool result = Owner.PerformTransitionBackToLatest();
                Debug.Log(result);
                return result;
            }
            return false;
        }


    }
}
