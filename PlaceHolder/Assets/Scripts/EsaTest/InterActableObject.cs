using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class InterActableObject : MonoBehaviour
    {
        [SerializeField, Tooltip("Items position in LevelControllers list")]
        private int m_iItemId;
        [SerializeField]
        private bool m_bIsActive;
        [SerializeField]
        private Inventory m_iInventory;

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();
        }

        /// <summary>
        /// Detects if mouse is over an object.
        /// </summary>
        private void OnMouseOver()
        {
            if (m_bIsActive)
            {
                // TODO Mouse animations
                if (Input.GetMouseButtonDown(0) && GameManager.instance.itemList[m_iItemId].Collected)
                {
                    Item item = GameManager.instance.itemList[m_iItemId];
                    m_iInventory.RemoveItem(item);
                }
            }
        }

    }
}