using ProjectThief.PathFinding;
using UnityEngine;

namespace ProjectThief
{
    public class ExitArea : ObjectBase
    {    
        [SerializeField, Tooltip("Inspect text")]
        private string _inspectText = @"""I might be able to escape through this window later.""";        

        private int _lastItem;

        private void Awake()
        {
            _lastItem = GameManager.instance.refItems.Count - 1;
        }

        protected override void Activated()
        {
            if (IsActive)
            {
                if (GameManager.instance.refItems[_lastItem].Collected)
                {

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