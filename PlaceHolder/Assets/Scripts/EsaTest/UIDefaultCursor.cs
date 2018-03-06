using UnityEngine.EventSystems;
using UnityEngine;

namespace ProjectThief
{
    public class UIDefaultCursor : MonoBehaviour, IPointerEnterHandler
    {
        private MouseController m_mcController;
        
        private void Awake()
        {
            m_mcController = GameManager.instance.mouseController;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            }
        }
    }
}