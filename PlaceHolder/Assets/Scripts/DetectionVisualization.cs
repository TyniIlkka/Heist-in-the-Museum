using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectThief
{
    public class DetectionVisualization : MonoBehaviour
    {
        [SerializeField]
        private float _maxViewRad;
        [SerializeField]
        private float _viewAngle;
        [SerializeField]
        private LayerMask _obstacleMask;
        [SerializeField]
        private float _meshResolution = 5;
        [SerializeField]
        private MeshFilter _meshFilter;
        [SerializeField]
        private float _edgeDistThreshold = 0.5f;
        [SerializeField]
        private int _edgeResolveIters = 6;
        [SerializeField, Tooltip("Lerp Duration")]
        private float _duration = 2f;

        private Mesh _mesh;
        private float _viewRad = 0;
        private float _startTime;
        private bool _detectStart;
        private bool _detectActive;
        private bool _detectStarted;
        private bool _detectEnded;
        private bool _playerFound;

        public Player _player;

        public float ViewRad { get { return _maxViewRad; } }
        public float ViewAngle { get { return _viewAngle; } }

        private void Awake()
        {
            Init();

            _mesh = new Mesh();
            _mesh.name = "View Mesh";
            _meshFilter.mesh = _mesh;
        }

        private void Init()
        {
            if (GetComponentInParent<Guard>() != null)
            {
                _maxViewRad = GetComponentInParent<Guard>().DetectionRange;
                _viewAngle = GetComponentInParent<Guard>().FieldOfView;
                _player = GetComponentInParent<Guard>().Thief;

            }
            else
            {
                Debug.LogError("ERROR: Guard not found.");
            }
        }

        public void DetectionActivated()
        {
            if (!_detectStarted)
            {
                _startTime = Time.time;
                _detectStart = true;
                _detectActive = true;
                _detectStarted = true;
                _detectEnded = false;
            }
        }

        public void DetectionDeactivated()
        {
            if (!_detectEnded)
            {
                _startTime = Time.time;
                _detectStart = false;
                _detectActive = true;
                _detectEnded = true;
                _detectStarted = false;
            }
        }

        private void DetectDistLerp()
        {
            if (GetComponentInParent<Guard>() != null)
            {
                if (_detectStart)
                {
                    float progress = Time.time - _startTime;
                    _viewRad = Mathf.Lerp(_viewRad, _maxViewRad, progress / _duration);                    
                }
                else if (!_detectStart && _viewRad != 0)
                {
                    float progress = Time.time - _startTime;
                    _viewRad = Mathf.Lerp(_viewRad, 0, progress / (_duration / 2));

                    if (_viewRad == 0)
                        _detectActive = false;
                }
            }
            else
            {
                Debug.LogError("ERROR: Guard not found.");
            }
        }

        private void Update()
        {
            Init();
            if (_detectActive && !_playerFound)
            {
                DetectDistLerp();
                DrawFieldOfView();
                if (CanSeePlayer() && GameManager.instance.canMove)
                {
                    _playerFound = true;
                    GameManager.instance.levelController.PlayerFound();
                }
            }
        }

        private void DrawFieldOfView()
        {
            int rayCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
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
                        Mathf.Abs(oldViewCast.dist - newViewCast.dist) > _edgeDistThreshold;

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

            _mesh.Clear();

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

        EdgeInfo FindEdge(ViewCastinfo minViewCast, ViewCastinfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < _edgeResolveIters; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastinfo newViewCast = ViewCast(angle);

                bool edgeDistThresholdExeeded =
                    Mathf.Abs(minViewCast.dist - newViewCast.dist) > _edgeDistThreshold;
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

            if (Physics.Raycast(transform.position, dir, out hit, _viewRad, _obstacleMask))
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

            Vector3 rayDirection = (_player.transform.position) - transform.position;
            float angle = Vector3.Angle(rayDirection, transform.forward);
            if (angle < _viewAngle * 0.5)
            {
                RaycastHit hit;
                rayDirection += Vector3.up;
                if (Physics.Raycast(transform.position, rayDirection.normalized, out hit, _viewRad))
                {
                    //Debug.DrawRay(transform.position, rayDirection.normalized);
                    //Debug.Log(hit.collider.gameObject);
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                    {

                        Debug.Log(hit.collider.gameObject);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}