using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Rooms items")]
        private List<Item> m_lItems;
        [SerializeField, Tooltip("Start point")]
        private Vector3 m_v3Point;
        [SerializeField, Tooltip("Player")]
        private GameObject m_goPlayer;

        private bool[] m_bHasItem;
        private bool m_bCleared;

        private void Awake()
        {
            
        }

        /// <summary>
        /// Room initialization method.
        /// </summary>
        public void Init()
        {
            m_goPlayer.transform.position = m_v3Point;

            if (!m_bCleared)
            {

            }
        }

        /// <summary>
        /// Called when room is resetted.
        /// </summary>
        public void ResetRoom()
        {
            for (int i = 0; i < m_lItems.Count; i++)
            {
                m_lItems[i].Collected = false;
            }
        }
    }
}