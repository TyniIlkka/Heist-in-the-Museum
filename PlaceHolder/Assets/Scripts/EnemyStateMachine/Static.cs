using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.WaypointSystem;

namespace ProjectThief.AI
{
    public class Static : AIStateBase
    {
        private MyDirections m_eDirection;

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
        }

        public override void Update()
        {
            // 1. Should we change the state?
            //   1.1 If yes, change state and return.

            if (!ChangeState())
            {
                m_eDirection = Owner.CurrentDirection;
                // 2. Stay on place.

                switch (m_eDirection)
                {
                    case MyDirections.North:
                        Owner.transform.forward = new Vector3(0f, 0f, 1f);
                        Owner.Direction = new Vector3(0f, 0f, 1f);
                        break;
                    case MyDirections.NorthEast:
                        Owner.transform.forward = new Vector3(1f, 0f, 1f);
                        Owner.Direction = new Vector3(1f, 0f, 1f);
                        break;
                    case MyDirections.East:
                        Owner.transform.rotation = Quaternion.Lerp(Owner.transform.rotation, Quaternion.Euler(1f,0f,0f), Time.deltaTime *Owner.TurnSpeed);
                        Owner.transform.forward = new Vector3(1f, 0f, 0f);
                        Owner.Direction = new Vector3(1f, 0f, 0f);
                        break;
                    case MyDirections.SouthEast:
                        Owner.transform.forward = new Vector3(1f, 0f, -1f);
                        Owner.Direction = new Vector3(1f, 0f, -1f);
                        break;
                    case MyDirections.South:
                        Owner.transform.forward = new Vector3(0f, 0f, -1f);
                        Owner.Direction = new Vector3(0f, 0f, -1f);
                        break;
                    case MyDirections.SouthWest:
                        Owner.transform.forward = new Vector3(-1f, 0f, -1f);
                        Owner.Direction = new Vector3(-1f, 0f, -1f);
                        break;
                    case MyDirections.West:
                        Owner.transform.forward = new Vector3(-1f, 0f, 0f);
                        Owner.Direction = new Vector3(-1f, 0f, 0f);
                        break;
                    case MyDirections.NorthWest:
                        Owner.transform.forward = new Vector3(-1f, 0f, 1f);
                        Owner.Direction = new Vector3(-1f, 0f, 1f);
                        break;
                }
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
