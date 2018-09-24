using System.Collections.Generic;
using UnityEngine;
using ProjectThief.AI;

namespace ProjectThief
{
    public class FOVClose : MonoBehaviour
    {
        [SerializeField]
        private float _viewRad;
        [SerializeField]
        private float _viewAngle = 360f;
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
        [SerializeField, Tooltip("Lerp duration")]
        private float _duration = 1f;

        private Mesh _viewMesh;
        private float _distanceToPlayer;
        private float _targetRad;
        private float _startRad;
        private float _startTime;
        private bool _lerpToRad;

        public Player _playerObject;
        private Guard guard;

        public float ViewRad { get { return _viewRad; } }
        public float ViewAngle { get { return _viewAngle; } }

        private void Awake()
        {
            Init();

            _viewMesh = new Mesh();
            _viewMesh.name = "View Mesh";
            m_mfViewMeshFilter.mesh = _viewMesh;
        }

        private void Init()
        {
            guard = GetComponentInParent<Guard>();
            if (guard != null)
            {
                _viewRad = GetComponentInParent<Guard>().MinDetectionRange;
                _targetRad = _viewRad;
                _viewAngle = 360f - GetComponent<FieldOfView>().ViewAngle;
            }
            else
            {
                Debug.LogError("ERROR: Guard not found.");
            }
        }

        private void CheckRadius()
        {
            if (guard != null)
            {
                if (_targetRad != guard.MinDetectionRange)
                {
                    _targetRad = guard.MinDetectionRange;
                    _startRad = _viewRad;
                    _lerpToRad = true;
                    _startTime = Time.time;
                }

                if (_lerpToRad)
                {
                    float progress = Time.time - _startTime;
                    _viewRad = Mathf.Lerp(_startRad, _targetRad, progress / _duration);

                    if (_viewRad == _targetRad)
                    {
                        _targetRad = _viewRad;
                        _lerpToRad = false;
                    }
                }
            }
            else
            {
                Debug.LogError("ERROR: Guard not found.");
            }            
        }

        private void CheckComponents()
        {
            if (guard == null)
                guard = GetComponentInParent<Guard>();
            if (_playerObject == null)
                _playerObject = GetComponentInParent<Guard>().Thief;            
        }

        private void LateUpdate()
        {
            CheckComponents();
            CheckRadius();
            DrawFieldOfView();
            if (CanSeePlayer())
            {
                Debug.Log("CanSeeClose");
                guard.PerformTransition(AIStateType.CloseTurnTo);
                Debug.Log(guard.PerformTransition(AIStateType.CloseTurnTo));

                //GameManager.instance.levelController.PlayerFound();
            }
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

            _viewMesh.Clear();

            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
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
            dir = -dir;
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

        public bool CanSeePlayer()
        {

            //Close range detection
            _distanceToPlayer = (transform.position - _playerObject.transform.position).sqrMagnitude;
            
            if ((_distanceToPlayer <= _viewRad * _viewRad))
            {
                Vector3 rayDirection = (_playerObject.transform.position) - transform.position;
                RaycastHit hit;
                rayDirection += Vector3.up;
                if (Physics.Raycast(transform.position, rayDirection.normalized, out hit, _viewRad))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}