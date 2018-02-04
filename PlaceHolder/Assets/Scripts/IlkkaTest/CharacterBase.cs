using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public abstract class CharacterBase : MonoBehaviour
    {
        /// <summary>
        /// Forces to every character to have Move Method();
        /// </summary>
        public abstract void Move(float amount);

    }
}