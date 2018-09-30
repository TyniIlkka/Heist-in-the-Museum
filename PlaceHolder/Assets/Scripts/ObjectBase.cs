using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public abstract class ObjectBase : MonoBehaviour
    {
        private float _volume;

        protected abstract void Activated();

        protected virtual void Update()
        {
            _volume = AudioManager.instance.SFXPlayVol;            
        }

        protected virtual void LateUpdate()
        {
            MouseCheck();
        }

        private void MouseCheck()
        {
            LayerMask hitLayers = GameManager.instance.rayCastLayers;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20, hitLayers) && !GameManager.instance.mouseOverUI)
            {
                if (hit.collider != null && hit.collider.GetComponent<ObjectBase>() != null)
                {
                    //Debug.Log("Object that was hit: " + hit.collider.gameObject.name);
                    ObjectBase hitObject = hit.collider.GetComponent<ObjectBase>();
                    hitObject.Activated();
                }
                else                
                    GetMouseController.DefaultCursor();                
            }
            else            
                GetMouseController.DefaultCursor();            
        }

        public MouseController GetMouseController { get { return GameManager.instance.mouseController; } }
        public bool IsActive { get; set; }
        public bool IsInteractable { get; set; }          
        public float PlayVolume { get { return _volume; } }
    }
}