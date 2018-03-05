using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class Floor : MonoBehaviour
    {
        private MouseController m_mcMouseController;

        private void Awake()
        {
            m_mcMouseController = GameManager.instance.mouseController;
        }

        private void OnMouseOver()
        {
            m_mcMouseController.MoveCursor();
        }

        private void OnMouseExit()
        {
            m_mcMouseController.DefaultCursor();
        }
    }
}