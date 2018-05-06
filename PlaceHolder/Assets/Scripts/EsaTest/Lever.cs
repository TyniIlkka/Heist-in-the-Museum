using UnityEngine;
using ProjectThief.PathFinding;

namespace ProjectThief
{
    public class Lever : ObjectBase
    {
        [SerializeField, Tooltip("Obstacle Animator")]
        private Animator _obstacleAnim;
        [SerializeField, Tooltip("Inventory object")]
        private Inventory _inventory;
        [SerializeField, Tooltip("Door")]
        private Door _door;
        [SerializeField, Tooltip("Marks if lever is broken")]
        private bool _isBroken;
        [SerializeField, Tooltip("Levers handle")]
        private GameObject _leverHandle;
        [SerializeField, Tooltip("Needed item")]
        private Item _neededItem;
        [SerializeField, Tooltip("Position in GM's bool list")]
        private int _boolListPos;
        [SerializeField, Tooltip("Levers used rotation")]
        private float _leverUsedRotation = -238;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPos;
        [SerializeField, Tooltip("Inspect text")]
        private string _inspectText = @"""A[colour] mechanism.""#""It looks like it is missing a part.""";

        private Animator _leverAnimator;
        private bool _used;

        public Vector3 MoveToPos { get { return _moveToPos.position; } }

        public bool trigger;

        private void Awake()
        {
            if (_inventory == null)
                _inventory = FindObjectOfType<Inventory>();

            _leverAnimator = GetComponent<Animator>();
            _neededItem = GameManager.instance.refItems[_neededItem.RefPos];

            Init();
        }        

        private void Init()
        {           
            if (GameManager.instance.usedlevers[_boolListPos])
            {
                _used = true;
                _leverHandle.SetActive(true);
                _leverAnimator.enabled = false;
                _leverHandle.transform.localEulerAngles = new Vector3(_leverUsedRotation,
                    _leverHandle.transform.localEulerAngles.y, _leverHandle.transform.localEulerAngles.z);                
                _door.Open = true;
                _door.Blocked = false;
            } 
            else
            {
                _leverHandle.SetActive(false);
                _isBroken = true;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.instance.usedlevers[_boolListPos] && trigger)
            {
                _leverHandle.transform.localEulerAngles = new Vector3(_leverUsedRotation,
                    _leverHandle.transform.localEulerAngles.y, _leverHandle.transform.localEulerAngles.z);
            }
        }

        /// <summary>
        /// Resets lever's and obstacle's animation
        /// </summary>
        public void ResetLever()
        {
            _leverAnimator.SetBool("Activated", false);
            _obstacleAnim.SetBool("Open", false);
        }

        protected override void Activated()
        {
            if (IsActive && !_used)
            {
                if (IsInteractable)
                {
                    if (_isBroken && _neededItem.Collected)
                    {
                        GetMouseController.InteractCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            _used = true;
                            _inventory.RemoveItem(_neededItem);
                            _leverHandle.SetActive(true);
                            _leverAnimator.SetBool("Activated", true);
                        }
                    }
                    else
                    {
                        GetMouseController.InspectCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            InspectText();
                        }
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();

                    if (Input.GetButtonDown("Fire1") && GameManager.instance.mouseMovemet)
                    {
                        GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                    }
                }
            }
        }

        public void OpenObstacle()
        {             
            _obstacleAnim.SetBool("Open", true);
            _door.ObstacleSound();
            _door.Open = false;
            _door.Blocked = false;
            GameManager.instance.usedlevers[_boolListPos] = true;            
        }

        private void InspectText()
        {
            GameManager.instance.infoText = _inspectText;

            if (!GameManager.instance.infoBoxVisible)
            {
                GameManager.instance.infoFadeIn = true;
                GameManager.instance.infoFadeInStart = true;
            }
            else
            {
                GameManager.instance.resetInfoTimer = true;
                GameManager.instance.newText = true;
            }
        }
    }    
}