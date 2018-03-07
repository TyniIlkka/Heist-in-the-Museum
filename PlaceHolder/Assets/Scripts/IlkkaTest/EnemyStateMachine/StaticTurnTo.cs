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
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.Static);
        }

        public override void StateActivated()
        {
            base.StateActivated();
            Debug.Log("käytiinkö täällä");

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
            int lightLayer = Owner.LightMask;

            Collider[] lights = Physics.OverlapSphere(Owner.transform.position,
                Owner.LightDetectDistance, lightLayer);
            foreach (Collider light in lights)
            {
                DistractLight lightSignal = light.GetComponent<DistractLight>();
                if (lightSignal.LightOn == true && lightSignal != null)
                {
                    Owner.TargetLight = lightSignal;
                    return Owner.PerformTransition(AIStateType.StaticTurnTo);
                }
            }
            return false;
        }

    }
}
