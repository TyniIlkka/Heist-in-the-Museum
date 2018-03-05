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

        public void MoveCursor()
        {
            Cursor.SetCursor(m_tMoveIcon, Vector2.zero, CursorMode.Auto);
        }

        public void InspectCursor()
        {
            Cursor.SetCursor(m_tInspectIcon, Vector2.zero, CursorMode.Auto);
        }

        public void EnterCursor()
        {
            Cursor.SetCursor(m_tEnterIcon, Vector2.zero, CursorMode.Auto);
        }

        public void InteractCursor()
        {
            Cursor.SetCursor(m_tInteractIcon, Vector2.zero, CursorMode.Auto);
        }

        public void DefaultCursor()
        {
            Cursor.SetCursor(m_tDefaultIcon, Vector2.zero, CursorMode.Auto);
        }        
    }
}