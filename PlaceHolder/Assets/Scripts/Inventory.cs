using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectThief
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField, Tooltip("Item slots")]
        private List<RawImage> _slots;
        [SerializeField, Tooltip("Slots highlights")]
        private List<RawImage> _highlights;
        [SerializeField, Tooltip("Empty slot texture")]
        private Texture _emptySlot = null;
        [SerializeField, Tooltip("Fadeout effect duration")]
        private float _duration = 1f;
        [SerializeField, Tooltip("How many highlight flashes")]
        private int _times = 2;

        private List<Item> _inventoryItems; 
        private int _slot;
        private bool _itemRemoved;
        private bool _itemAdded;
        private bool _effectStarted;
        private RawImage _highlightImage;
        private float _r, _g, _b;
        private float _startTime;
        private int _done;
        private int _usedSlots;

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
            _slot = _usedSlots;
            _inventoryItems.Add(newItem);
            _itemAdded = true;
            UpdateInventory();
        }

        /// <summary>
        /// Removes item from inventory.
        /// </summary>
        public void RemoveItem(Item item)
        {
            Item removeItem = GameManager.instance.refItems[item.RefPos];
            _inventoryItems.Remove(removeItem);
            _slot = item.Slot;
            _slots[_slot].GetComponent<RawImage>().texture = _emptySlot;
            _itemRemoved = true;
        }

        /// <summary>
        /// Updates Displayed Inventory.
        /// </summary>
        public void UpdateInventory()
        {
            _usedSlots = 0;
            for (int i = 0; i < _slots.Count; i++)
            {                
                if (i >= _inventoryItems.Count)
                {                    
                    _slots[i].GetComponent<RawImage>().texture = _emptySlot;                    
                }
                else
                {
                    _slots[i].GetComponent<RawImage>().texture = _inventoryItems[i].ItemImage;
                    _inventoryItems[i].Slot = i;
                    _usedSlots++;
                }
            }            
        }        

        // Update method for testing delay.
        private void Update()
        {
            if (!_effectStarted && (_itemAdded || _itemRemoved))
            {
                _effectStarted = true;
                _highlightImage = _highlights[_slot].GetComponent<RawImage>();
                _r = _highlightImage.color.r;
                _g = _highlightImage.color.g;
                _b = _highlightImage.color.b;
                _startTime = Time.time;
                _highlightImage.color = new Vector4(_r, _g, _b, 1);
                _done = 0;
            }

            if (_effectStarted)
            {
                SlotHighlight();

                if (_highlightImage.color.a == 0 && _done < _times)
                {
                    _done++;

                    if (_done < _times)
                    {
                        _startTime = Time.time;
                        _highlightImage.color = new Vector4(_r, _g, _b, 1);
                    }
                }

                if (_highlightImage.color.a == 0 && _done == _times)
                {
                    _effectStarted = false;
                    if (_itemAdded)
                        _itemAdded = false;
                    if (_itemRemoved)
                    {
                        _itemRemoved = false;
                        UpdateInventory();
                    }                    
                }           
            }
        }

        private void SlotHighlight()
        {
            float progress = Time.time - _startTime;
            _highlightImage.color = Color.Lerp(_highlightImage.color, new Vector4(_r, _g, _b, 0), progress / _duration);
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