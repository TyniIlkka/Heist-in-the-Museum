using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField]
        private float m_fViewRad;
        [SerializeField]
        private float m_fViewAngle;        
        [SerializeField]
        private LayerMask m_lmObstacleMask;
        [SerializeField]
        private float m_fMeshResolution = 5;
        [SerializeField]
        private MeshFilter m_mfViewMeshFilter;
        [SerializeField]
        private float m_fEdgeDistThreshold = 0.5f;
        [SerializeField]
        private int m_iEdgeResolveIters = 6;
        [SerializeField, Tooltip("Light distance mult")]
        private float m_fMult = 2;
        [SerializeField, Tooltip("Flashlight")]
        private Light m_lLight;
        [SerializeField, Tooltip("Detection visualization script")]
        private DetectionVisualization _detVis;

        private Mesh m_mViewMesh;
        private bool _playerFound;

        public Player m_pPlayerObject;

        public float ViewRad { get { return m_fViewRad; } }
        public float ViewAngle { get { return m_fViewAngle; } }

        private void Awake()
        {
            Init(); 

            m_mViewMesh = new Mesh();
            m_mViewMesh.name = "View Mesh";
            m_mfViewMeshFilter.mesh = m_mViewMesh;

            if (_detVis == null)
                _detVis = GetComponent<DetectionVisualization>();
        }               

        private void Init()
        {
            if (GetComponentInParent<Guard>() != null)
            {
                m_fViewRad = GetComponentInParent<Guard>().DetectionRange;
                m_fViewAngle = GetComponentInParent<Guard>().FieldOfView;
                m_pPlayerObject = GetComponentInParent<Guard>().Thief;

            }
            else
            {
                Debug.LogError("ERROR: Guard not found.");
            }

            if (m_lLight != null)
            {
                m_lLight.range = m_fViewRad * m_fMult;
                m_lLight.spotAngle = m_fViewAngle;
            }
            else
            {
                Debug.LogError("ERROR: Light not found.");
            }
        }

        private void Update()
        {
            Init();
            DrawFieldOfView();

            if (CanSeePlayer() && GameManager.instance.canMove)
                _detVis.DetectionActivated();

            else if (!CanSeePlayer() && _playerFound)
            {
                _detVis.DetectionDeactivated();
                _playerFound = false;
            }
        }

        private void DrawFieldOfView()
        {
            int rayCount = Mathf.RoundToInt(m_fViewAngle * m_fMeshResolution);
            float rayAngleSize = m_fViewAngle / rayCount;

            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastinfo oldViewCast = new ViewCastinfo();

            for (int i = 0; i <= rayCount; i++)
            {
                float angle = transform.eulerAngles.y - m_fViewAngle / 2 + rayAngleSize * i;
                ViewCastinfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDistThresholdExeeded = 
                        Mathf.Abs(oldViewCast.dist - newViewCast.dist) > m_fEdgeDistThreshold;

                    if (oldViewCast.hit != newViewCast.hit || 
                        (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExeeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.pointA != Vector3.zero)
                            viewPoints.Add(edge.pointA);
                        if (edge.pointB != Vector3.zero)
                            viewPoints.Add(edge.pointB);
                    }
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount -1; i++)
            { 
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);                

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            m_mViewMesh.Clear();

            m_mViewMesh.vertices = vertices;
            m_mViewMesh.triangles = triangles;
            m_mViewMesh.RecalculateNormals();
        }

        EdgeInfo FindEdge(ViewCastinfo minViewCast, ViewCastinfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < m_iEdgeResolveIters; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastinfo newViewCast = ViewCast(angle);

                bool edgeDistThresholdExeeded = 
                    Mathf.Abs(minViewCast.dist - newViewCast.dist) > m_fEdgeDistThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExeeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        ViewCastinfo ViewCast (float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;                      

            if (Physics.Raycast(transform.position, dir, out hit, m_fViewRad, m_lmObstacleMask))
                return new ViewCastinfo(true, hit.point, hit.distance, globalAngle);

            else
                return new ViewCastinfo(false, transform.position + dir * m_fViewRad, hit.distance, globalAngle);
        }

        public Vector3 DirFromAngle (float angleInDeg, bool globalAngle)
        {
            if (!globalAngle)
                angleInDeg += transform.eulerAngles.y;

            Vector3 result = new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
            return result;
        }

        public struct ViewCastinfo
        {
            public bool hit;
            public Vector3 point;
            public float dist;
            public float angle;

            public ViewCastinfo(bool _hit, Vector3 _point, float _dist, float _angle)
            {
                hit = _hit;
                point = _point;
                dist = _dist;
                angle = _angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }

        public bool CanSeePlayer()
        {
            
            Vector3 rayDirection = (m_pPlayerObject.transform.position) - transform.position;
            float angle = Vector3.Angle(rayDirection, transform.forward);
            if (angle < m_fViewAngle * 0.5)
            {
                RaycastHit hit;
                rayDirection += Vector3.up;
                if (Physics.Raycast(transform.position, rayDirection.normalized, out hit, m_fViewRad))
                {
                    //Debug.DrawRay(transform.position, rayDirection.normalized);
                    //Debug.Log(hit.collider.gameObject);
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                    {
                        _playerFound = true;
                        Debug.Log(hit.collider.gameObject);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}