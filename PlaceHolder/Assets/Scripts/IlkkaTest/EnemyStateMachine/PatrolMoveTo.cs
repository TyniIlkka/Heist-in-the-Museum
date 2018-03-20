using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        public PatrolMoveTo(Guard owner)
            : base()
        {
            State = AIStateType.PatrolMoveTo;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveFrom);
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

                Pathing.FindPath(Owner.transform.position, Owner.TargetSound.transform.position);


                //3. Move the finded way

                //TODO: add animation trigger
                //Owner.MoveAnimation(Path);
                if (Path.Count > 0)
                {
                    MoveMethod();
                }
            }
        }

        public IEnumerable WaitTillMoveBack()
        {
            yield return new WaitForSeconds(Owner.WaitTime);
        }

        /// <summary>
        /// Change state, this state change when object is close enough current waypoint.
        /// </summary>
        /// <returns>Bool result</returns>
        private bool ChangeState()
        {
            if (Owner.TargetSound == null)
            {
                Debug.Log("Liikutaan pois hämäyksestä");
                bool result = Owner.PerformTransition(AIStateType.PatrolMoveFrom);
                return result;
            }
            return false;
        }


    }
}
