using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.PathFinding;

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

        [SerializeField]
        private Transform target;

        [SerializeField]
        private PathGridManager m_Grid;
        private List<Node> m_lPath;

        private void Awake()
        {
            m_lPath = m_Grid.Path;
        }

        // Update is called once per frame
        void Update()
        {
            m_lPath = m_Grid.Path;
            
            Move(m_lPath);
        }

        /// <summary>
        /// Move method to 
        /// </summary>
        public override void Move(List<Node>_lPathToFollow)
        {
            for (int i = 0; i <= _lPathToFollow.Count; i++)
            {
                
            }
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
    }
}
