using System.Collections;
using System.Collections.Generic;
//using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        //public Nodes DistractNode { get; private set; }

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

                Owner.Move(Owner.transform.forward);
                Owner.Turn(Owner.TargetSound.transform.position);

            }
        }

        //private Nodes GetDistractionNode()
        //{
        //    return null;
        //}

        private bool ChangeState()
        {
            if (!Owner.Distracted)
            {
                Debug.Log("Hämättyliikkuvaa");
                bool result = Owner.PerformTransition(AIStateType.PatrolMoveFrom);
                return result;
            }
            return false;
        }

    }
}
