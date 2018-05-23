using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _defaultIcon;
        [SerializeField]
        private Texture2D _moveIcon;
        [SerializeField]
        private Texture2D _inspectIcon;
        [SerializeField]
        private Texture2D _enterIcon;
        [SerializeField]
        private Texture2D _interactIcon;                

        public void MoveCursor()
        {
            Cursor.SetCursor(_moveIcon, Vector2.zero, CursorMode.Auto);
        }

        public void InspectCursor()
        {
            Cursor.SetCursor(_inspectIcon, Vector2.zero, CursorMode.Auto);
        }

        public void EnterCursor()
        {
            Cursor.SetCursor(_enterIcon, Vector2.zero, CursorMode.Auto);
        }

        public void InteractCursor()
        {
            Cursor.SetCursor(_interactIcon, Vector2.zero, CursorMode.Auto);
        }

        public void DefaultCursor()
        {
            Cursor.SetCursor(_defaultIcon, Vector2.zero, CursorMode.Auto);
        }        
    }
}