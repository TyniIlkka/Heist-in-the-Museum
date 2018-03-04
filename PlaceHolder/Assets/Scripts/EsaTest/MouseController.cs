using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField]
        private Texture2D m_tDefaultIcon;
        [SerializeField]
        private Texture2D m_tMoveIcon;
        [SerializeField]
        private Texture2D m_tInspectIcon;
        [SerializeField]
        private Texture2D m_tEnterIcon;
        [SerializeField]
        private Texture2D m_tInteractIcon;

        private bool m_bMove;
        private bool m_bInspect;
        private bool m_bEnter;
        private bool m_bInteract;

        public bool Moving { set { m_bMove = value; } }
        public bool Inspect { set { m_bInspect = value; } }
        public bool Enter { set { m_bEnter = value; } }
        public bool Interact { set { m_bInteract = value; } }

        // Update is called once per frame
        void Update()
        {
            UpdateMouseIcon();
        }

        private void UpdateMouseIcon()
        {
            Texture2D cursorTexture;

            if (m_bMove)
                cursorTexture = m_tMoveIcon;
            if (m_bInspect)
                cursorTexture = m_tInspectIcon;
            if (m_bEnter)
                cursorTexture = m_tEnterIcon;
            if (m_bInteract)
                cursorTexture = m_tInteractIcon;
            else
                cursorTexture = m_tDefaultIcon;

            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}