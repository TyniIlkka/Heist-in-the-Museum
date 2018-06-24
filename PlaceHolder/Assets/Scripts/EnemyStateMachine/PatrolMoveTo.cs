using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectThief.PathFinding;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        private IEnumerator coroutine;
        public bool ready;

        public PatrolMoveTo(Guard owner)
            : base()
        {
            State = AIStateType.PatrolMoveTo;
            Owner = owner;
            
            AddTransition(AIStateType.PatrolStayAtTarget);
            AddTransition(AIStateType.CloseTurnTo);
        }

        public override void StateActivated()
        {
            base.StateActivated();
            Mover = Owner.GetComponent<GuardMover>();

            ready = false;
        }

        public override void StateDeactivating()
        {
            base.StateDeactivating();
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                //2. 
                if (!ready)
                {
                    Mover.Target = Owner.TargetSound.MoveToPos;
                    Mover.FindPath(Owner.transform.position, Owner.TargetSound.MoveToPos);
                }
                
                if (Mover.MoverPath.Count > 0)
                {
                    if (GameManager.instance.canMove)
                    {
                        MoveMethod();
                    }
                    
                    if (Mover.MoverPath.Count == 1)
                    {
                        ready = true;
                    }
                    
                }

            }
        }

        /// <summary>
        /// Change state, this state change when object is close enough current waypoint.
        /// </summary>
        /// <returns>Bool result</returns>
        private bool ChangeState()
        {
            if (Mover.MoverPath.Count <= 0 && ready)
            {
                bool result = Owner.PerformTransition(AIStateType.PatrolStayAtTarget);
                return result;
            }
            return false;
        }


    }
}
