using UnityEngine;

namespace ProjectThief
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField]
        private float m_fViewRad;
        [SerializeField]
        private float m_fViewAngle;

        public float ViewRad { get { return m_fViewRad; } }

        private void Awake()
        {
            // TODO Get view radius and angle from guard;
        }

        public Vector3 DirFromAngle (float angleInDeg)
        {
            Vector3 result = new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
            return result;
        }
    }
}