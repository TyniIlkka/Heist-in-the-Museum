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
        private float m_fMeshResolution;
        [SerializeField]
        private MeshFilter m_mfViewMeshFilter;
        [SerializeField]
        private float m_fEdgeDistThreshold;
        [SerializeField]
        private int m_iEdgeResolveIters;

        private Mesh m_mViewMesh;


        public float ViewRad { get { return m_fViewRad; } }
        public float ViewAngle { get { return m_fViewAngle; } }

        private void Awake()
        {
            if (GetComponent<Guard>() != null)
            {
                m_fViewRad = GetComponent<Guard>().DetectionRange;
                m_fViewAngle = GetComponent<Guard>().FieldOfView;
            }
            else
            {
                Debug.LogError("404 Guard not found.");
            }

            m_mViewMesh = new Mesh();
            m_mViewMesh.name = "View Mesh";
            m_mfViewMeshFilter.mesh = m_mViewMesh;
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
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
    }
}