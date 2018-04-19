using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using ProjectThief.PathFinding;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        public PatrolMoveTo(Guard owner)
            : base()
        {
            State = AIStateType.PatrolMoveTo;
            Owner = owner;
            
            AddTransition(AIStateType.Patrol);
        }

        public override void StateActivated()
        {
            base.StateActivated();
            Mover = Owner.GetComponent<GuardMover>();
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                //2. Find the way to the current way point

                //Mover.FindPath(Owner.transform.position, )

                Mover.Target = Owner.TargetSound.transform.position;


                //3. Move the finded way

                //TODO: add animation trigger
                //Owner.MoveAnimation(Path);
                if (Path.Count > 0)
                {
                    MoveMethod();
                    if (Path.Count == 1 && Owner.Distracted)
                    {
                        WaitTillMoveBack();
                        Owner.Distracted = false;
                        Mover.Target = Owner.CurrentWaypoint.transform.position;
                    }
                    
                }
            }
        }

        public IEnumerable WaitTillMoveBack()
        {
            yield return new WaitForSeconds(Owner.WaitTime);
            Owner.TargetSound = null;
        }

        /// <summary>
        /// Change state, this state change when object is close enough current waypoint.
        /// </summary>
        /// <returns>Bool result</returns>
        private bool ChangeState()
        {
            if (Path.Count <= 0 && Owner.Distracted == false)
            {
                Debug.Log("Liikutaan pois hämäyksestä");
                bool result = Owner.PerformTransition(AIStateType.Patrol);
                return result;
            }
            return false;
        }


    }
}
