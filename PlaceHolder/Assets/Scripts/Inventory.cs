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

        private List<Item> _inventoryItems;        

        // testing remove when not needed.
        public float delay;
        private bool start; 
        private float time;

        public List<Item> InventoryItems { get { return _inventoryItems; } }

        private void Start()
        {
            _inventoryItems = GameManager.instance.inventory;                       
            UpdateInventory();
        }

        /// <summary>
        /// Adds item into inventory.
        /// </summary>
        public void AddItem(Item item)
        {            
            Item newItem = GameManager.instance.refItems[item.RefPos];
            newItem.Collected = true;
            _inventoryItems.Add(newItem);
            UpdateInventory();
        }

        /// <summary>
        /// Removes item from inventory.
        /// </summary>
        public void RemoveItem(Item item)
        {
            Item removeItem = GameManager.instance.refItems[item.RefPos];
            _inventoryItems.Remove(removeItem);            
            m_lSlots[item.Slot].GetComponent<RawImage>().texture = m_tEmpty;        
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
                if (i >= _inventoryItems.Count)
                {                    
                    m_lSlots[i].GetComponent<RawImage>().texture = m_tEmpty;                    
                }
                else
                {
                    m_lSlots[i].GetComponent<RawImage>().texture = _inventoryItems[i].ItemImage;
                    _inventoryItems[i].Slot = i;
                }
            }            
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

        public void SaveInventory()
        {
            GameManager.instance.SaveInventory(_inventoryItems);
        }

        public void LoadInventory()
        {
            if (_inventoryItems == null)
            {
                _inventoryItems = GameManager.instance.inventory;
                _inventoryItems.Clear();
            }

            foreach (Item item in GameManager.instance.savedInventory)
            {
                Item loadedItem = GameManager.instance.refItems[item.RefPos];
                _inventoryItems.Add(loadedItem);
            }
        }
    }
}