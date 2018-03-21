using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief.AI
{
    public enum AIStateType
    {
        Error = 0,
        Patrol = 1,
        PatrolMoveTo = 2,
        PatrolMoveFrom = 3,
        Static = 4,
        StaticTurnTo = 5
    }

    public abstract class AIStateBase
    {
        // The state related to this object.
        public AIStateType State { get; protected set; }
        // The target states to which we can transition from this state.
        public IList<AIStateType> TargetStates { get; protected set; }
        // The owner Unit of this state (Unit is the state controller class)
        public Guard Owner { get; protected set; }

        public GuardMover Mover { get; set; }

        public List<Vector3> Path = new List<Vector3>();

        protected AIStateBase()
        {
            TargetStates = new List<AIStateType>();
        }

        // A constructor which sets the Owner and State properties and calles
        // the default constructor.
        protected AIStateBase(Guard owner, AIStateType state)
            : this()
        {
            Owner = owner;
            State = state;
        }

        /// <summary>
        /// Add a valid state to which we can go from this state.
        /// </summary>
        /// <param name="targetState">The target state</param>
        /// <returns>True, if the state was added succesfully (not present in our 
        /// datastructure already). False otherwise.</returns>
        public bool AddTransition(AIStateType targetState)
        {
            // Use the extension method AddUnique to add a target state. Will return false
            // if the state was already added.
            return TargetStates.AddUnique(targetState);
        }

        /// <summary>
        /// Removes a target state from TargetStates data structure.
        /// </summary>
        /// <param name="targetState">The state to be removed.</param>
        /// <returns>True, if the target state was succesfully removed from the 
        /// data structure. False otherwise.</returns>
        public bool RemoveTransition(AIStateType targetState)
        {
            return TargetStates.Remove(targetState);
        }

        /// <summary>
        /// Checks if it is legal to go from this state to the target state.
        /// </summary>
        /// <param name="targetState">The target state to go to.</param>
        /// <returns>True, if the transition is legal, false otherwise.</returns>
        public virtual bool CheckTransition(AIStateType targetState)
        {
            return TargetStates.Contains(targetState);
        }

        /// <summary>
        /// Called just after the state is activated.
        /// </summary>
        public virtual void StateActivated()
        {
            
        }

        /// <summary>
        /// Called just before state is deactivated.
        /// </summary>
        public virtual void StateDeactivating()
        {
        }

        /// <summary>
        /// Called every frame the AI system is in this state.
        /// </summary>
        public abstract void Update();

        public void MoveMethod()
        {
            if (Mover.Path.Count > 0)
            {
                Vector3 direction = (Mover.Path[0] - Owner.transform.position).normalized;

                float step = Owner.TurnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(Owner.transform.forward, direction, step, 0.0F);
                newDir.y = 0;
                //newDir.z = 0;
                Owner.transform.rotation = Quaternion.LookRotation(newDir);
                //transform.LookAt(newDir);

                Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, Owner.transform.position + direction, Time.deltaTime * Owner.MoveSpeed);
                if (Owner.transform.position.x < Mover.Path[0].x + 0.4F && Owner.transform.position.x > Mover.Path[0].x - 0.4F && Owner.transform.position.z > Mover.Path[0].z - 0.4F && Owner.transform.position.z < Mover.Path[0].z + 0.4F)
                {
                    Mover.Path.RemoveAt(0);
                }

                RaycastHit[] hit = Physics.RaycastAll(Owner.transform.position + (Vector3.up * 20F), Vector3.down, 100);
                float maxY = -Mathf.Infinity;
                foreach (RaycastHit h in hit)
                {
                    if (h.transform.tag == "Untagged")
                    {
                        if (maxY < h.point.y)
                        {
                            maxY = h.point.y;
                        }
                    }
                }
                Owner.transform.position = new Vector3(Owner.transform.position.x, maxY, Owner.transform.position.z);
            }
        }
    }
}

