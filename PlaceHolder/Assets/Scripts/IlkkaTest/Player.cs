using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief {
    public class Player : CharacterBase
    {

        //Testing different movements boolean
        public bool m_bRotateWithKeyboard;


        [SerializeField]
        private string m_sMouseAxisX = "Mouse X";
        [SerializeField]
        private string m_sHorizontalAxis = "Horizontal";
        [SerializeField]
        private string m_sVerticalAxis = "Vertical";

        [SerializeField]
        private float m_fMovementSpeed;
        [SerializeField]
        private float m_fTurnSpeed;


        // Update is called once per frame
        void Update()
        {
            var input = ReadInput();
            if (m_bRotateWithKeyboard)
            {
                Turn(input.z);
            }
            else
            {
                MoveLeftRight(input.z);
            }
            Move(input.x);

        }

        /// <summary>
        /// Move method to 
        /// </summary>
        public override void Move(float amount)
        {
            Vector3 position = transform.position;
            Vector3 movement = transform.forward * amount * m_fMovementSpeed * Time.deltaTime;
            position += movement;
            transform.position = position;
        }


        /// <summary>
        /// Turn method
        /// </summary>
        public void Turn(float amount)
        {
            Vector3 rotation = transform.localEulerAngles;
            rotation.y += amount * m_fTurnSpeed * Time.deltaTime;
            transform.localEulerAngles = rotation;
        }



        /// <summary>
        /// Move left or right depending from test boolean
        /// </summary>
        public void MoveLeftRight(float amount)
        {
            Vector3 position = transform.position;
            Vector3 movement = transform.right * amount * m_fMovementSpeed * Time.deltaTime;
            position += movement;
            transform.position = position;
        }

        /// <summary>
        /// Input from player
        /// </summary>
        /// <returns>Vector3</returns>
        private Vector3 ReadInput()
        {
            float movement = Input.GetAxis(m_sHorizontalAxis);
            float turn = Input.GetAxis(m_sVerticalAxis);
            return new Vector3(turn, 0, movement);
        }

    }
}
