using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectThief.PathFinding;

namespace ProjectThief.AI
{
    public class PatrolMoveBack : AIStateBase
    {
        private IEnumerator coroutine;
        public bool ready;

        public PatrolMoveBack(Guard owner)
            : base()
        {
            State = AIStateType.PatrolMoveBack;
            Owner = owner;

            AddTransition(AIStateType.Patrol);
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
                //2. Find the way to the current way point
                //Mover.FindPath(Owner.transform.position, )

                //Owner.TargetSound.transform.position.y = 0;
                if (!ready)
                {
                    Mover.Target = Owner.CurrentWaypoint.transform.position;
                    Mover.FindPath(Owner.transform.position, Owner.CurrentWaypoint.transform.position);
                }



                //3. Move the finded way

                //TODO: add animation trigger
                //Owner.MoveAnimation(Path);
                if (Mover.MoverPath.Count > 0)
                {

                    if (GameManager.instance.canMove)
                    {
                        MoveMethod();
                    }
                    if (Mover.MoverPath.Count == 1)
                    {

                        Owner.DistractedSound = false;
                        Debug.Log("Etsittiin path takaisin " + Mover.MoverPath.Count);
                        ready = true;
                    }

                }

            }
        }

        IEnumerable WaitTillMoveBack()
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
            if (Mover.MoverPath.Count <= 0 && ready)
            {
                Debug.Log("Liikutaan pois hämäyksestä");
                bool result = Owner.PerformTransition(AIStateType.Patrol);
                return result;
            }
            return false;
        }


    }
}
