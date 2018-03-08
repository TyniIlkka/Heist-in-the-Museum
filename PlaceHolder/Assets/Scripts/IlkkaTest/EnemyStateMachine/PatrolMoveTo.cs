using System.Collections;
using System.Collections.Generic;
using ProjectThief.PathFinding;
using UnityEngine;
using System;

namespace ProjectThief.AI
{
    public class PatrolMoveTo : AIStateBase
    {
        public Node DistractNode { get; private set; }

        public PatrolMoveTo(Guard owner)
            : base()
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.PatrolMoveTo);
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
               
                

            }
        }

        private Node GetDistractionNode()
        {
            return null;
        }

        private bool ChangeState()
        {
            //int mask = LayerMask.GetMask( "Player" );
            int soundLayer = LayerMask.NameToLayer("SoundOutput");

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.SoundDetectDistance, soundLayer);
            if (players.Length > 0)
            {
                
            }
            return false;
        }

    }
}
