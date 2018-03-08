using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief {
    public class Player : CharacterBase
    { 
        private Animator m_aPlayerAnimator;

        public override void Init()
        {
            Player player = GetComponent<Player>();
            m_aPlayerAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            FindPath();
            MoveAnimation();
            if (Path.Count > 0)
            {
                Move();
            }
        }

        private void FindPath()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    FindPath(transform.position, hit.point);
                }
            }
        }

        public void ResetPath()
        {

        }

        private void MoveAnimation()
        {
            if Path.Count > 0)
            {
                m_aPlayerAnimator.SetBool("Moving", true);
                Debug.Log("Moving");
            }
            else
            {
                m_aPlayerAnimator.SetBool("Moving", false);
                Debug.Log("Idle");
            }         
        }
    }
}
