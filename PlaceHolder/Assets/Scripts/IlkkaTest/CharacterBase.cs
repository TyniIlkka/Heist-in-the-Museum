using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public abstract class CharacterBase : MonoBehaviour
    {

        private Animator anim;
        /// <summary>
        /// Forces to every character to have Move Method();
        /// </summary>
        public abstract void Move(Vector3 direction);
        public abstract void Turn(Vector3 amount);

        public Animator Animation {
            get {return anim ;}
            set { anim = value; }
        }

        protected void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            anim = GetComponent<Animator>();
        }
    }
}