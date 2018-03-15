using System.Collections;
using System.Collections.Generic;
//using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveFrom : AIStateBase
    {
        //public Nodes DistractNode { get; private set; }

        public PatrolMoveFrom(Guard owner)
            : base()
        {
            State = AIStateType.PatrolMoveFrom;
            Owner = owner;
            AddTransition(AIStateType.Patrol);
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

                Owner.Move(Owner.transform.forward);
                Owner.Turn(Owner.TargetSound.transform.position);

            }
        }

        private bool ChangeState()
        {
            bool result = false;
            //TODO: are we close enough the current waypoint.
            if (result)
            {
                
                return result;
            }
            return false;
        }

    }
}
