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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _activationDist);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactDist);
        }

        private void Awake()
        {
            _objects = GameManager.instance.levelController.InteractableObjects;
        }

        private void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (_objects.Count > 0)
            {
                for (int i = 0; i < _objects.Count; i++)
                {
                    ObjectBase obj = _objects[i];

                    if (obj.CheckDistance)
                    {
                        float distance = Vector3.Distance(transform.position, obj.transform.position);

                        if (distance <= _activationDist)
                        {
                            obj.IsActive = true;

                            if (distance <= _interactDist)
                                obj.IsInteractable = true;
                            else
                                obj.IsInteractable = false;
                        }
                        else
                            obj.IsActive = false;
                    }
                }
            }
        }
    }
}