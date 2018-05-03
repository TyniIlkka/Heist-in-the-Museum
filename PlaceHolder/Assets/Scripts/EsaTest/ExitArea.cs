using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class ExitArea : ObjectBase
    {       
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;

        public Vector3 MoveToPos { get { return _moveToPoint.position; } }

        private int _lastItem;

        private void Awake()
        {
            _lastItem = GameManager.instance.refItems.Count - 1;
            Debug.Log("Item needed: " + _lastItem);
            Debug.Log("Monkey collected: " + GameManager.instance.refItems[_lastItem].Collected);
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                Debug.Log("end area is active");
                if (GameManager.instance.refItems[_lastItem].Collected)
                {
                    Debug.Log("Escape possible");
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            GameManager.instance.canMove = false;
                            GameManager.instance.inTransit = true;
                            GameManager.instance.levelController.PlayerEscaped();
                        }
                    }
                    else
                    {
                        GetMouseController.InspectCursor();

                        if (Input.GetButtonDown("Fire1"))
                        {
                            GameManager.instance.player.GetComponent<GridPlayer>().FindPath(MoveToPos);
                        }
                    }
                }
                else
                {
                    GetMouseController.InspectCursor();
                    Debug.Log("Escape impossible");
                }
            }
            Debug.Log("end area is no longer active");
        }
    }
}