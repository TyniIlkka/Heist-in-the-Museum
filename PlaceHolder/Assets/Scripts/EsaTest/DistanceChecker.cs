using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    [RequireComponent(typeof(Player))]
    public class DistanceChecker : MonoBehaviour
    {
        [SerializeField, Tooltip("Object activation distance")]
        private float _activationDist = 10f;
        [SerializeField, Tooltip("Object interaction distance")]
        private float _interactDist = 5f;
        [SerializeField]
        private LayerMask mask;

        private List<ObjectBase> _objects = new List<ObjectBase>();
        private List<ObjectBase> _toBeRemoved = new List<ObjectBase>();

        public Collider[] objects;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _activationDist);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactDist);
        }

        // Call this only when player moves?

        private void Update()
        {
            GetObjectsInRange();
            CheckDistance();
        }

        private void GetObjectsInRange()
        {
            
            objects = Physics.OverlapSphere(transform.position, _activationDist, mask);
            
            if (objects.Length > 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {

                    // TODO Check if object is in currently active room.
                    if (objects[i].GetComponent<ObjectBase>() != null &&
                        !_objects.Contains(objects[i].GetComponent<ObjectBase>()))
                    {
                        objects[i].GetComponent<ObjectBase>().IsActive = true;
                        _objects.Add(objects[i].GetComponent<ObjectBase>());

                        if (objects[i].GetComponent<Item>() != null)
                            objects[i].GetComponent<Item>().HighLightItem();
                    }                    
                }
            }
        }

        private void CheckDistance()
        {
            if (_objects.Count > 0)
            {
                for (int i = 0; i < _objects.Count; i++)
                {
                    ObjectBase obj = _objects[i];
                    
                    if (Vector3.Distance(transform.position, obj.transform.position) <= _interactDist)
                    {
                        obj.IsInteractable = true;
                    }
                    if (Vector3.Distance(transform.position, obj.transform.position) > _interactDist)
                    {
                        obj.IsInteractable = false;
                    }
                    if (Vector3.Distance(transform.position, obj.transform.position) > _activationDist)
                    {
                        obj.IsActive = false;

                        if (obj.GetComponent<Item>() != null)
                            obj.GetComponent<Item>().DeHighLight();

                        _toBeRemoved.Add(obj);                       
                    }
                }

                if (_toBeRemoved.Count > 10)
                {
                    RemoveItems();
                }
            }
        }

        private void RemoveItems()
        {
            for (int i = 0; i < _toBeRemoved.Count; i++)
            {
                ObjectBase item = _toBeRemoved[i];
                _objects.Remove(item);
            }

            _toBeRemoved.Clear();
        }
    }
}