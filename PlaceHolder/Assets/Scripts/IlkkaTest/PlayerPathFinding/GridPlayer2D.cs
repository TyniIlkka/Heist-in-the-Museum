using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectThief.PlayerPathFinding
{
    public class GridPlayer2D : Pathfinding2D
    {
        [SerializeField]
        private float m_fMovementSpeed;

        void Update()
        {
            FindPath();
            if (Path.Count > 0)
            {
                Move(m_fMovementSpeed);
            }
        }

        private void FindPath()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,  Input.mousePosition.z, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    FindPath(transform.position, hit.point);
                }
            }
        }
    }
}
