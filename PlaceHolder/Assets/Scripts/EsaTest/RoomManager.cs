using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Rooms items")]
        private List<Item> m_lItems;
        [SerializeField, Tooltip("Door to room")]
        private Door m_dDoor;
        
        private bool m_bCleared; 

        /// <summary>
        /// Called when room is resetted.
        /// </summary>
        public void ResetRoom()
        {
            GameObject player = GameManager.instance.player;

            if (!m_bCleared) {
                for (int i = 0; i < m_lItems.Count; i++)
                {
                    m_lItems[i].Collected = false;
                }
            }

            player.transform.position = m_dDoor.RoomPos;
        }
    }
}