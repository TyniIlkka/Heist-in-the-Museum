using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public abstract class ObjectBase : MonoBehaviour
    {
        protected abstract void OnMouseOver();
        protected abstract void OnMouseExit();

        public bool IsActive { get; set; }
        public bool IsInteractable { get; set; }
        // TODO add boolean for moving towards object.
    }
}