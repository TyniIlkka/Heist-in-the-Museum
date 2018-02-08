using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief.PathFinding
{
    public class A_Star : MonoBehaviour
    {

        public Transform m_tSeeker, m_tTarget;
        PathGridManager m_Grid;

        List<Node> m_lPath;
        [SerializeField]
        public Node currentNode;

        private void Awake()
        {
            m_Grid = GetComponent<PathGridManager>();
        }

        // Update is called once per frame
        void Update()
        {
            FindPath(m_tSeeker.position, m_tTarget.position);
        }

        public int GetDistance(Node a, Node b)
        {
            int iDistX = Mathf.Abs(a.m_iGridX - b.m_iGridX);
            int iDistY = Mathf.Abs(a.m_iGridY - b.m_iGridY);

            if (iDistX > iDistY)
            {
                return 14 * iDistY + 10 * (iDistX - iDistY);
            }
            else
            {
                return 14 * iDistX + 10 * (iDistY - iDistX);
            }
        }

        public void FindPath(Vector3 startPoint, Vector3 endPoint)
        {


            Node startNode = m_Grid.NodeFromWorldPos(startPoint);
            Node endNode = m_Grid.NodeFromWorldPos(endPoint);

            List<Node> m_lOpenSet = new List<Node>();
            List<Node> m_lClosedSet = new List<Node>();

            //G-Cost
            startNode.m_iGCost = GetDistance(startNode, startNode);
            //H-Cost
            startNode.m_iHCost = GetDistance(startNode, endNode);

            m_lOpenSet.Add(startNode);

            Node currentNode = null;
            List<Node> neighbours = null;

            while (m_lOpenSet.Count > 0)
            {
                currentNode = LowestFCostNode(m_lOpenSet);
                m_lOpenSet.Remove(currentNode);
                m_lClosedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    Retrace(startNode, endNode);
                    return;
                }

                neighbours = m_Grid.GetNeighbours(currentNode);

                foreach (Node neighbour in neighbours)
                {
                    if (neighbour.m_bIsBlocked)
                        continue;
                    if (m_lClosedSet.Contains(neighbour))
                        continue;

                    int _iNewMovementCost = currentNode.m_iGCost + GetDistance(currentNode, neighbour);

                    if (_iNewMovementCost < neighbour.m_iGCost || !m_lOpenSet.Contains(neighbour))
                    {
                        neighbour.m_iGCost = _iNewMovementCost;
                        neighbour.m_iHCost = GetDistance(neighbour, endNode);
                        neighbour.m_nParent = currentNode;

                        if (!m_lOpenSet.Contains(neighbour))
                        {
                            m_lOpenSet.Add(neighbour);
                        }
                    }
                }
            }

            Debug.Log("No Route!");
        }

        public Node LowestFCostNode(List<Node> nodes)
        {
            Node result = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                if ((nodes[i].m_ifCost < result.m_ifCost) || (nodes[i].m_ifCost == result.m_ifCost && nodes[i].m_iHCost < result.m_iHCost))
                {
                    result = nodes[i];
                }
            }
            return result;
        }

        public void Retrace(Node _nStartPoint, Node _nEndPoint)
        {
            //Debug.Log("Takaisin jäljitys.");
            m_lPath = new List<Node>();
            currentNode = _nEndPoint;

            while (currentNode != _nStartPoint)
            {
                m_lPath.Add(currentNode);
                currentNode = currentNode.m_nParent;
            }
            m_lPath.Reverse();

            //m_Grid.PathMakeClear(path);
            m_Grid.Path = m_lPath;
            if (m_Grid.Path == null)
            {
                Debug.Log("path puuttuuu");
            }
        }
    }
}
