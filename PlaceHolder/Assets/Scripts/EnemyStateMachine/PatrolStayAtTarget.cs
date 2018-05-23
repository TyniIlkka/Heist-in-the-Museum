using System.Collections;
using System.Collections.Generic;
using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolStayAtTarget : AIStateBase
    {
        private float timer;

        public PatrolStayAtTarget(Guard owner)
            : base()
        {
            State = AIStateType.PatrolStayAtTarget;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveBack);
            AddTransition(AIStateType.CloseTurnTo);
        }

        public override void StateActivated()
        {
            base.StateActivated();
            timer = Owner.TargetSound.DistractionTime;
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                if (GameManager.instance.canMove)
                {
                    //2. Look at target.
                    Vector3 lookToPosition = Owner.TargetSound.transform.position;
                    lookToPosition.y = 0f;
                    Owner.transform.LookAt(lookToPosition, Vector3.up);

                    timer -= Time.deltaTime;
                }
                
            }
        }

        private bool ChangeState()
        {
            //Is timer < 0
            if (timer <= 0)
            {
                bool result = Owner.PerformTransition(AIStateType.PatrolMoveBack);
                return result;
            }
            return false;
        }

    }
}
