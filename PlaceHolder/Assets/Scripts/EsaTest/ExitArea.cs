using UnityEngine;

namespace ProjectThief
{
    public class ExitArea : ObjectBase
    {
        [SerializeField, Tooltip("Treasure pos in ref list")]
        private int m_iPos = 12;

        protected override void Activated()
        {
            if (IsActive)
            {
                if (GameManager.instance.refItems[m_iPos].Collected)
                {
                    if (IsInteractable)
                    {
                        GetMouseController.EnterCursor();
                        if (Input.GetMouseButton(0))
                        {
                            GameManager.instance.levelController.PlayerEscaped();
                        }
                    }
                }
            }
        }
    }
}