using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectThief.PathFinding
{
    public class GridPlayer : Pathfinding
    {
        public Camera playerCam;
        public Camera minimapCam;

        public float m_fMoveSpeed;
        public float m_fTurnSpeed;

        public Player player;


        private void Start()
        {
            player = GetComponent<Player>();
            playerCam = Camera.main;
            if (m_fMoveSpeed <= 0) m_fMoveSpeed = 1;
        }

        //public GUIStyle bgStyle;

        void Update()
        {
            
            FindPath();
            player.MoveAnimation(Path);
            if (Path.Count > 0)
            {
                MoveMethod();
            }
        }

        private void FindPath()
        {

            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = playerCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    FindPath(transform.position, hit.point);
                }
            }
        }

        private void MoveMethod()
        {
            if (Path.Count > 0)
            {
                Vector3 direction = (Path[0] - transform.position).normalized;

                float step = m_fTurnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0F);
                newDir.y = 0;
                //newDir.z = 0;
                transform.rotation = Quaternion.LookRotation(newDir);
                //transform.LookAt(newDir);

                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * m_fMoveSpeed);
                if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
                {
                    Path.RemoveAt(0);
                }

                RaycastHit[] hit = Physics.RaycastAll(transform.position + (Vector3.up * 20F), Vector3.down, 100);
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
                transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            }
        }

        void OnGUI()
        {
            
            //GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", bgStyle);
        }
    }
}