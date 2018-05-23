using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectThief.PathFinding
{
    public class DynamicMapUpdate : MonoBehaviour
    {
        public float UpdateTimer = 0.5F;
        private Vector3 lastPosition;
        Bounds lastBounds;

        void Start()
        {
            lastPosition = transform.position;
            lastBounds = GetComponent<Renderer>().bounds;
            UpdateMapOnce();
            StartCoroutine(UpdateMap());
        }

        IEnumerator UpdateMap()
        {
            if (transform.position != lastPosition)
            {
                Bounds bR = GetComponent<Renderer>().bounds;
                Pathfinder.Instance.DynamicRaycastUpdate(lastBounds);
                Pathfinder.Instance.DynamicRaycastUpdate(bR);
                lastPosition = transform.position;
                lastBounds = bR;
            }

            yield return new WaitForSeconds(UpdateTimer);
            StartCoroutine(UpdateMap());
        }

        public void UpdateMapOnce()
        {
            Bounds bR = GetComponent<Renderer>().bounds;
            Pathfinder.Instance.DynamicRaycastUpdate(lastBounds);
            Pathfinder.Instance.DynamicRaycastUpdate(bR);
            lastPosition = transform.position;
            lastBounds = bR;
        }

        void OnDestroy()
        {
            UpdateMapOnce();
        }
    }
}