using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class PlayerFoV : MonoBehaviour
    {
        [SerializeField]
        private float _viewRad;
        [SerializeField]
        private float _viewAngle = 360;
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
        [SerializeField]
        private float _duration = 1;
        [SerializeField, Tooltip("Footprint detection range")]
        private float _range;

        private Player _player;
        private Mesh m_mViewMesh;
        private float _startRad;
        private float _targetRad;
        private bool _initDone;
        private float _startTime;

        private void Awake()
        {
            Init();

            m_mViewMesh = new Mesh();
            m_mViewMesh.name = "View Mesh";
            m_mfViewMeshFilter.mesh = m_mViewMesh;
        }

        private void Init()
        {
            if (GetComponentInParent<Player>() != null && !_player)
            {
                _player = GetComponentInParent<Player>();
                _viewRad = 0f;
                _targetRad = _viewRad;
                _initDone = true;
            }
            else
            {
                Debug.LogError("ERROR: Player not found.");
            }
        }

        private void Update()
        {
            CheckRadius();
        }

        private void CheckRadius()
        {
            if (_player)
            {
                if (_player.Sneaking && _targetRad != _range)
                {
                    _targetRad = _range;
                    _startRad = _viewRad;
                    _startTime = Time.time;
                }
                else if (!_player.Sneaking && _targetRad != 0)
                {
                    _targetRad = 0;
                    _startRad = _viewRad;
                    _startTime = Time.time;
                }

                if (_viewRad != _targetRad)
                {
                    float progress = Time.time - _startTime;
                    _viewRad = Mathf.Lerp(_startRad, _targetRad, progress / _duration);

                    if (_viewRad == _targetRad)
                        _targetRad = _viewRad;
                }                          
            }
        }

        private void LateUpdate()
        {
            if (!_initDone)
                Init();

            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            int rayCount = Mathf.RoundToInt(_viewAngle * m_fMeshResolution);
            float rayAngleSize = _viewAngle / rayCount;

            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastinfo oldViewCast = new ViewCastinfo();

            for (int i = 0; i <= rayCount; i++)
            {
                float angle = transform.eulerAngles.y - _viewAngle / 2 + rayAngleSize * i;
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
            for (int i = 0; i < vertexCount - 1; i++)
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

        ViewCastinfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, _viewRad, m_lmObstacleMask))
                return new ViewCastinfo(true, hit.point, hit.distance, globalAngle);

            else
                return new ViewCastinfo(false, transform.position + dir * _viewRad, hit.distance, globalAngle);
        }

        public Vector3 DirFromAngle(float angleInDeg, bool globalAngle)
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