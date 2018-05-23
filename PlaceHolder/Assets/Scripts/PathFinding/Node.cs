//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace ProjectThief.PlayerPathFinding {
//    public class Nodes
//    {

//        public bool m_bIsBlocked;
//        public Vector3 m_vPosition;

//        public int m_iGridX;
//        public int m_iGridY;

//        public int m_iGCost;
//        public int m_iHCost;

//        public Node m_nParent;
//        public bool m_bIsPath;

//        public int m_ifCost
//        {
//            get { return m_iGCost + m_iHCost; }
//        }

//        public Nodes(bool bIsBlocked, Vector3 vPos, int x, int y, bool _bIsPath)
//        {
//            m_bIsBlocked = bIsBlocked;
//            m_vPosition = vPos;
//            m_iGridX = x;
//            m_iGridY = y;
//            m_bIsPath = _bIsPath;
//        }

//    }
//}