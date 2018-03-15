using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief.AI
{
    public class StaticTurnTo : AIStateBase
    {
        public StaticTurnTo(Guard owner)
            : base()
        {
            State = AIStateType.StaticTurnTo;
            Owner = owner;
            AddTransition(AIStateType.Static);
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
                // 2. Turn to Target light
                Owner.Turn(Owner.TargetLight.transform.position);
            }
        }

        

        private bool ChangeState()
        {
            if (!Owner.Distracted)
            {
                Debug.Log("Hämätty");
                bool result = Owner.PerformTransition(AIStateType.StaticTurnTo);
                return result;
            }
            return false;
        }

    }
}
