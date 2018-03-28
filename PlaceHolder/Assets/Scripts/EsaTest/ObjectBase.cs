using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public abstract class ObjectBase : MonoBehaviour
    {
        protected abstract void OnMouseOver();
        protected abstract void OnMouseExit();        

        public MouseController GetMouseController { get { return GameManager.instance.mouseController; } }
        public bool IsActive { get; set; }
        public bool IsInteractable { get; set; }                
    }
}