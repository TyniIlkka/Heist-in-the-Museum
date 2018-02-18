using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectThief
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private List<RawImage> m_lSlots;
        [SerializeField]
        private Texture m_tEmpty = null; 

        private List<Item> m_lInventoryItems = new List<Item>();
                
        /// <summary>
        /// Adds item into inventory.
        /// </summary>
        public void AddItem(Item item)
        {
            m_lInventoryItems.Add(item);
            UpdateInventory();
        }

        /// <summary>
        /// Removes item from inventory.
        /// </summary>
        public void RemoveItem(Item item)
        {
            m_lInventoryItems.Remove(item);
            // TODO Add use Animation and use animation to call updateInventory
            UpdateInventory();
        }

        /// <summary>
        /// Updates Displayed Inventory.
        /// </summary>
        private void UpdateInventory()
        {
            for (int i = 0; i < m_lSlots.Count; i++)
            { 
                if (i >= m_lInventoryItems.Count)
                {
                    m_lSlots[i].GetComponent<RawImage>().texture = m_tEmpty;
                }
                else
                {
                    m_lSlots[i].GetComponent<RawImage>().texture = m_lInventoryItems[i].ItemImage;
                }
            }
        }        
    }
}