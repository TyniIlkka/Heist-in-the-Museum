using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI
{
    public class Static : AIStateBase
    {
        private MyDirections m_eDirection;
        private int dir;

        public Static(Guard owner, MyDirections currentDirection)
            : base()
        {
            State = AIStateType.Static;
            Owner = owner;
            AddTransition(AIStateType.StaticTurnTo);
            AddTransition(AIStateType.Patrol);
            AddTransition(AIStateType.CloseTurnTo);
            m_eDirection = currentDirection;

        }

        public override void StateActivated()
        {
            base.StateActivated();
            m_eDirection = Owner.CurrentDirection;
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                Vector3 target = Owner.directionList[(int)m_eDirection].position;
                Owner.Turn(target);
            }
        }

        private bool ChangeState()
        {
            if (Owner.DistractedLight)
            {
                Debug.Log("Hämätty");
                bool result = Owner.PerformTransition(AIStateType.StaticTurnTo);
                return result;
            }
            if (Owner.Moving)
            {
                bool result = Owner.PerformTransition(AIStateType.Patrol);
            }
            return false;
        }

        

    }
}