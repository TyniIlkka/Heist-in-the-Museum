using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    [RequireComponent(typeof(Player))]
    public class DistanceChecker : MonoBehaviour
    {
        [SerializeField, Tooltip("Object activation distance")]
        private float m_fActivate = 10f;
        [SerializeField, Tooltip("Object interaction distance")]
        private float m_fInteract = 5f;

        private List<ObjectBase> m_lObjects = new List<ObjectBase>();
        private List<ObjectBase> m_lItemRemove = new List<ObjectBase>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_fActivate);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_fInteract);
        }

        // Call this only when player moves?

        private void Update()
        {
            GetObjectsInRange();
            CheckDistance();
        }

        private void GetObjectsInRange()
        {            
            Collider[] objects = Physics.OverlapSphere(transform.position, m_fActivate);
            
            if (objects.Length > 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {

                    // TODO Check if object is in currently active room.
                    if (objects[i].GetComponent<ObjectBase>() != null &&
                        !m_lObjects.Contains(objects[i].GetComponent<ObjectBase>()))
                    {
                        objects[i].GetComponent<ObjectBase>().IsActive = true;
                        m_lObjects.Add(objects[i].GetComponent<ObjectBase>());                        
                    }                    
                }
            }
        }

        private void CheckDistance()
        {
            if (m_lObjects.Count > 0)
            {
                for (int i = 0; i < m_lObjects.Count; i++)
                {
                    ObjectBase obj = m_lObjects[i];

                    if (Vector3.Distance(transform.position, obj.gameObject.transform.position) <= m_fInteract)
                    {
                        obj.IsInteractable = true;
                    }
                    else if (Vector3.Distance(transform.position, obj.gameObject.transform.position) > m_fInteract)
                    {
                        obj.IsInteractable = false;
                    }
                    else if (Vector3.Distance(transform.position, obj.gameObject.transform.position) > m_fInteract)
                    {
                        obj.IsActive = false;
                        m_lItemRemove.Add(obj);
                    }
                }

                if (m_lItemRemove.Count > 0)
                {
                    RemoveItems();
                }
            }
        }

        private void RemoveItems()
        {
            for (int i = 0; i < m_lItemRemove.Count; i++)
            {
                ObjectBase item = m_lItemRemove[i];
                m_lObjects.Remove(item);
            }

            m_lItemRemove.Clear();
        }
    }
}