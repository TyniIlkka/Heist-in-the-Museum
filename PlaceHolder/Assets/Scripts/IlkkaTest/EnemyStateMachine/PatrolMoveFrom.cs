using System.Collections;
using System.Collections.Generic;
using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveFrom : AIStateBase
    {
        //public Nodes DistractNode { get; private set; }

        public PatrolMoveFrom(Guard owner, GuardMover mover)
            : base()
        {
            State = AIStateType.PatrolMoveFrom;
            Owner = owner;
            Mover = mover;
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
                //2. Find the way to the current way point

                Mover.FindPath(Owner.transform.position, Owner.CurrentWaypoint.transform.position);


                //3. Move the finded way

                //TODO: add animation trigger
                //Owner.MoveAnimation(Path);
                if (Mover.Path.Count > 0)
                {
                    MoveMethod();
                }
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
