using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class ExitArea : ObjectBase
    {
        [SerializeField, Tooltip("Treasure pos in ref list")]
        private int m_iPos = 12;
        [SerializeField, Tooltip("Move to point")]
        private Transform _moveToPoint;

        public Vector3 MoveToPos { get { return _moveToPoint.position; } }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (GameManager.instance.refItems[m_iPos].Collected)
                {
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (Input.GetButtonDown("Fire1"))
                        {
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
                    GetMouseController.InspectCursor();
            }
        }
    }
}