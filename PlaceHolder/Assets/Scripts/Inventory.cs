﻿using System.Collections;
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

        private List<Item> m_lInventoryItems;        

        // testing remove when not needed.
        public float delay;
        private bool start; 
        private float time;

        private void Start()
        {
            m_lInventoryItems = GameManager.instance.inventory;
                       
            UpdateInventory();
        }

        /// <summary>
        /// Adds item into inventory.
        /// </summary>
        public void AddItem(Item item)
        {
            GameManager.instance.collectedItems[item.Position] = true;
            Item newItem = GameManager.instance.refItems[item.RefPos];
            m_lInventoryItems.Add(newItem);            
            UpdateInventory();
        }

        /// <summary>
        /// Removes item from inventory.
        /// </summary>
        public void RemoveItem(Item item)
        {
            Item removeItem = GameManager.instance.refItems[item.RefPos];
            m_lInventoryItems.Remove(removeItem);            
            m_lSlots[item.Slot].GetComponent<RawImage>().texture = m_tEmpty;
            // TODO Add use Animation and use animation to call updateInventory
            //UpdateInventory();
            time = delay;
            start = true;            
        }

        /// <summary>
        /// Updates Displayed Inventory.
        /// </summary>
        public void UpdateInventory()
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
                    m_lInventoryItems[i].Slot = i;
                }
            }

            GameManager.instance.CheckInventory();
        }

        // Update method for testing delay.
        private void Update()
        {
            if (start)
            {
                if (time <= 0)
                {
                    start = false;
                    UpdateInventory();
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }
    }
}