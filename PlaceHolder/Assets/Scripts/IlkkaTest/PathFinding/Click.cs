using UnityEngine;
using System.Collections;

namespace ProjectThief.PathFinding
{
    public class Click : MonoBehaviour
    {
        public string moveButton = "Fire 2";
        public string interactButton = "Fire 1";

        public float shootDistance = 10f;
        public float shootRate = .5f;

        private Animator anim;
        private PathGridManager pathGridManager;
        private Transform targetedEnemy;
        private Ray shootRay;
        private bool walking;
        private Transform target;

        public Camera mainCamera = Camera.main;

        private float pointerY = 1f;

        // Use this for initialization
        void Awake()
        {
            target = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

            target.transform.position = mainCamera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, pointerY, Input.mousePosition.z));
            if (Input.GetButtonDown("Fire2"))
            {
                
              
            }

            //anim.SetBool("IsWalking", walking);
        }

        private void Move()
        {
            
        }

    }

}