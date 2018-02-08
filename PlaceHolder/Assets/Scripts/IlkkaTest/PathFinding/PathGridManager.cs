using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectThief.PathFinding
{
    public class PathGridManager : MonoBehaviour
    {

        public Transform m_tPlayerTransform;
        public Transform m_tTargetTransform;
        public LayerMask m_ObstacleMask;
        public Vector2 m_vGridSize;
        public float m_fHalfNodeWidth;

        Node[,] m_aGrid;

        Node m_nCurrentNode;
        //List<Node> m_lNeighbours;
        List<Node> m_aPath;
        List<Node> m_aPathOld;

        public List<Node> Path
        {
            get { return m_aPath; }
            set { m_aPath = value; }
        }

        float m_fNodeWidth;
        int m_iNumNodesX, m_iNumNodesY;

        private void Start()
        {
            m_aPath = new List<Node>();
            // How many nodes are in the grid?
            m_fNodeWidth = m_fHalfNodeWidth * 2;
            m_iNumNodesX = Mathf.RoundToInt(m_vGridSize.x / m_fNodeWidth);
            m_iNumNodesY = Mathf.RoundToInt(m_vGridSize.y / m_fNodeWidth);

            CreateGrid();

        }
        private void Update()
        {
            //PathCheck();
        }

        void CreateGrid()
        {
            // Create Grid
            m_aGrid = new Node[m_iNumNodesX, m_iNumNodesY];
            Vector3 vGridBotLeft = transform.position - (Vector3.right * m_vGridSize.x / 2) - (Vector3.forward * m_vGridSize.y / 2);

            // Detect if node is blocked
            for (int x = 0; x < m_iNumNodesX; ++x)
            {
                for (int y = 0; y < m_iNumNodesY; ++y)
                {
                    Vector3 vNodePos = vGridBotLeft + Vector3.right * (x * m_fNodeWidth + m_fHalfNodeWidth) + Vector3.forward * (y * m_fNodeWidth + m_fHalfNodeWidth);
                    bool bIsBlocked = (Physics.CheckSphere(vNodePos, m_fHalfNodeWidth, m_ObstacleMask));

                    m_aGrid[x, y] = new Node(bIsBlocked, vNodePos, x, y, false);
                }
            }
        }

        public Node NodeFromWorldPos(Vector3 vWorldPos)
        {
            float fX = (vWorldPos.x + m_vGridSize.x / 2) / m_vGridSize.x;
            float fY = (vWorldPos.z + m_vGridSize.y / 2) / m_vGridSize.y;

            fX = Mathf.Clamp01(fX);
            fY = Mathf.Clamp01(fY);

            int x = Mathf.RoundToInt((m_iNumNodesX - 1) * fX);
            int y = Mathf.RoundToInt((m_iNumNodesY - 1) * fY);

            return m_aGrid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int iCheckX = node.m_iGridX + i;
                    int iCheckY = node.m_iGridY + j;

                    if (iCheckX >= 0 && iCheckX < m_vGridSize.x && iCheckY >= 0 && iCheckY < m_vGridSize.y)
                    {
                        neighbours.Add(m_aGrid[iCheckX, iCheckY]);
                    }
                }
            }
            return neighbours;
        }

        public void PathMakeClear(List<Node> path)
        {
            if (m_aPathOld != null)
            {
                foreach (Node item in m_aPathOld)
                {
                    item.m_bIsPath = false;
                }
            }

            foreach (Node node in path)
            {
                node.m_bIsPath = true;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(m_vGridSize.x, 1, m_vGridSize.y));

            if (Path != null)
            {
                if (m_aPathOld != null)
                {
                    foreach (Node item in m_aPathOld)
                    {
                        item.m_bIsPath = false;
                    }
                }

                foreach (Node node in Path)
                {
                    node.m_bIsPath = true;
                    m_aPathOld = Path;
                }
            }
            if (Path == null)
            {
                Debug.LogError("Path is Null!");
            }

            if (Path.Count < 0)
            {
                Debug.LogError("Path is Null!");
            }
            if (m_aGrid != null)
            {

                Node playerNode = NodeFromWorldPos(m_tPlayerTransform.position);
                Node targetNode = NodeFromWorldPos(m_tTargetTransform.position);
                foreach (Node node in m_aGrid)
                {

                    Gizmos.color = (node.m_bIsBlocked) ? Color.red : Color.white;
                    if (node.m_bIsPath == true) Gizmos.color = Color.blue;
                    if (node == playerNode) Gizmos.color = Color.black;
                    if (node == targetNode) Gizmos.color = Color.black;
                    Gizmos.DrawWireCube(node.m_vPosition, Vector3.one * (m_fNodeWidth - .1f));
                }
            }
        }
    }
}