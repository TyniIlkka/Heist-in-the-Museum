using ProjectThief.PathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Lever : ObjectBase
    {
        [SerializeField, Tooltip("Obstacle Animator")]
        private Animator m_aObstacleAnim;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory m_iInventory;
        [SerializeField, Tooltip("Door")]
        private Door m_dDoor;
        [SerializeField, Tooltip("Marks if lever is broken")]
        private bool m_bBroken;
        [SerializeField, Tooltip("Levers handle")]
        private GameObject m_goHandle;
        [SerializeField, Tooltip("Needed item")]
        private Item m_itNeededItem;
        [SerializeField, Tooltip("Position in GM's bool list")]
        private int m_iPos;
        [SerializeField, Tooltip("Levers used rotation")]
        private float m_fRoationX = -238;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPos;

        private Animator m_aLeverAnim;
        private bool m_bUsed;

        public Vector3 MoveToPos { get { return _moveToPos.position; } }

        public bool trigger;

        private void Awake()
        {
            if (m_iInventory == null)
                m_iInventory = FindObjectOfType<Inventory>();

            m_aLeverAnim = GetComponent<Animator>();
            m_itNeededItem = GameManager.instance.refItems[m_itNeededItem.RefPos];

            Init();
        }        

        private void Init()
        {           
            if (GameManager.instance.usedlevers[m_iPos])
            {
                m_bUsed = true;
                m_goHandle.SetActive(true);
                m_aLeverAnim.enabled = false;
                m_goHandle.transform.localEulerAngles = new Vector3(m_fRoationX,
                    m_goHandle.transform.localEulerAngles.y, m_goHandle.transform.localEulerAngles.z);                
                m_dDoor.Open = true;
                m_dDoor.Blocked = false;
            } 
            else
            {
                m_goHandle.SetActive(false);
                m_bBroken = true;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.instance.usedlevers[m_iPos] && trigger)
            {
                m_goHandle.transform.localEulerAngles = new Vector3(m_fRoationX,
                    m_goHandle.transform.localEulerAngles.y, m_goHandle.transform.localEulerAngles.z);
            }
        }

        /// <summary>
        /// Resets lever's and obstacle's animation
        /// </summary>
        public void ResetLever()
        {
            m_aLeverAnim.SetBool("Activated", false);
            m_aObstacleAnim.SetBool("Open", false);
        }

        protected override void Activated()
        {
            if (IsActive && !m_bUsed)
            {
                if (IsInteractable)
                {
                    if (m_bBroken && m_itNeededItem.Collected)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            m_bUsed = true;
                            m_iInventory.RemoveItem(m_itNeededItem);
                            m_goHandle.SetActive(true);
                            m_aLeverAnim.SetBool("Activated", true);
                        }
                    }
                    else
                    {
                        GetMouseController.InspectCursor();
                        // TODO inform player that they are missing something.
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();

                    //if (Input.GetButtonDown("Fire1"))
                    //{
                    //    GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                    //}
                }
            }
            else if (m_bUsed)
            {
                // Tell player that they have used it already
            }
        }

        public void OpenObstacle()
        {             
            m_aObstacleAnim.SetBool("Open", true);
            m_dDoor.ObstacleSound();
            m_dDoor.Open = false;
            m_dDoor.Blocked = false;
            GameManager.instance.usedlevers[m_iPos] = true;            
        }
    }    
}