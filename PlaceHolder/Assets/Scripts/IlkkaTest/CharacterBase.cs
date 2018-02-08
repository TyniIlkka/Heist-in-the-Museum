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
        public abstract void Move(float amount);


        public void Init()
        {
            anim = GetComponent<Animator>();
        }
    }
}