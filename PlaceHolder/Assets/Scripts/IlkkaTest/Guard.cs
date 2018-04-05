using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using ProjectThief.AI;
using ProjectThief.PathFinding;
using ProjectThief.WaypointSystem;

namespace ProjectThief {

    public enum MyDirections
    {
        Error = -1,
        North ,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public class Guard : CharacterBase {

        [SerializeField]
        private Player player;

        #region States
        Patrol patrol;
        PatrolMoveTo patrolMoveTo;
        PatrolMoveFrom patrolMoveFrom;
        Static guardStatic;
        StaticTurnTo staticTurnTo;

        #endregion
        

        [SerializeField, Header("Patrolling or Static"), Tooltip("if True, guard is moving, otherwise static")]
        private bool m_bMoving;


        #region Moving, turning, pathfinding

        [SerializeField]
        private float m_fMovementSpeed;

        [SerializeField]
        private float m_fTurnSpeed;

        [SerializeField]
        private float m_fWaitTime;

        private GuardMover guardMover;

        #region StaticGuard
        [Header("StaticGuard")]
        [SerializeField, Range(0, 40), Tooltip("How far can sounds distract guards:")]
        private float m_fLightDetectDistance;
        [SerializeField]
        private LayerMask m_lmLightMask;

        private bool m_bDistracted;


        #endregion

        #region Patrol
        [Header("Patrol")]
        [SerializeField, Range(0, 40), Tooltip("How far can sounds distract guards:")]
        private float m_fSoundDetectDistance;
        
        [SerializeField]
        private LayerMask m_lmSoundMask;
        //Set Path
        [SerializeField]
        private List<PathPoints> _paths;
        //How smooth guards is going to corner.
        [SerializeField]
        private float _waypointArriveDistance;
        //Which way are we moving.
        [SerializeField]
        private Direction _direction;

        private int _currenPathNumber = 0;

        
        
        #endregion
        #endregion
        #region Detecting variables.
        [Header("Detecting Player")]
        [SerializeField, Tooltip("Which Direction Guard is looking if not patrolling: ")]
        private MyDirections m_eDirection;
        private Vector3 m_vDirection;

        [SerializeField]
        private LayerMask m_lmDetectHitMask;
        [SerializeField, Range(0,20), Tooltip("How far guard detect on facing direction.")]
        private float m_fMaxDetectionRange;
        [SerializeField, Range(0, 10), Tooltip("How close guard detect to every direction.")]
        private float m_fMinDetectionRange;

        [SerializeField, Range(0, 90), Tooltip("FieldOfView of guard.")]
        private float m_fFieldOfView;

        [SerializeField, Tooltip("Position for static guard.")]
        private Vector3 m_vPosition;
        #endregion

        #region Constructors
        public float TurnSpeed
        {
            get { return m_fTurnSpeed; }
            set {m_fTurnSpeed = value; }
        }
        public float MoveSpeed
        {
            get { return m_fMovementSpeed; }
            set { m_fMovementSpeed = value; }
        }
        public float WaitTime
        {
            get { return m_fWaitTime; }
            set { m_fWaitTime = value; }
        }
        public float FieldOfView
        {
            get { return m_fFieldOfView; }
            private set { } 
        }
        public float DetectionRange
        {
            get { return m_fMaxDetectionRange; }
            private set { }
        }
        public Waypoint CurrentWaypoint { get; set; }

        #region Distract

        public float LightDetectDistance
        {
            get { return m_fLightDetectDistance; }
        }
        public float SoundDetectDistance
        {
            get { return m_fSoundDetectDistance; }
        }
        public LayerMask SoundMask
        {
            get { return m_lmSoundMask; }
        }
        public LayerMask LightMask
        {
            get { return m_lmLightMask; }
        }
        public bool Distracted
        {
            get { return m_bDistracted; }
            set { m_bDistracted = value; }
        }
        #endregion

        #region StateMachine
        //CurrentDirection
        public MyDirections CurrentDirection
        {
            get { return m_eDirection; }
            set { m_eDirection = value; }
        }
        // Vector Direction
        public Vector3 Direction
        {
            get { return m_vDirection; }
            set { m_vDirection = value; }
        }
        //List of states
        private IList<AIStateBase> _states = new List<AIStateBase>();

        public AIStateBase CurrentState { get; set; }
        // The player unit this enemy is trying to shoot at.
        public LightDistraction TargetLight { get; set; }
        public SoundDistraction TargetSound { get; set; }
        #endregion
        #endregion

        public override void Init()
        {
            // Runs the base classes implementation of the Init method.
            base.Init();
            guardMover = GetComponent<GuardMover>();
            // Initializes the state system.
            InitStates();
        }

        /// <summary>
        /// Init States.
        /// </summary>
        private void InitStates()
        {
            patrol = new Patrol(this, _paths, _direction, _waypointArriveDistance, _currenPathNumber );
            _states.Add(patrol);

            patrolMoveTo = new PatrolMoveTo(this, guardMover);
            _states.Add(patrolMoveTo);

            patrolMoveFrom = new PatrolMoveFrom(this,guardMover);
            _states.Add(patrolMoveFrom);

            guardStatic = new Static(this, CurrentDirection);
            _states.Add(guardStatic);

            staticTurnTo = new StaticTurnTo(this);
            _states.Add(staticTurnTo);

            CheckCurrentState();
            CurrentState.StateActivated();

            Debug.Log(CurrentState);
        }

        /// <summary>
        /// Sets CurrentState.
        /// </summary>
        private void CheckCurrentState()
        {
            if (m_bMoving)
            {
                CurrentState = patrol;
            }
            else if (!m_bMoving)
            {
                CurrentState = guardStatic;
            }
        }
        /// <summary>
        /// Change guards state to another.
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns></returns>
        public bool PerformTransition(AIStateType targetState)
        {
            if (!CurrentState.CheckTransition(targetState))
            {
                return false;
            }

            bool result = false;

            AIStateBase state = GetStateByType(targetState);
            if (state != null)
            {
                CurrentState.StateDeactivating();
                CurrentState = state;
                CurrentState.StateActivated();
                result = true;
                Debug.Log(CurrentState);
            }

            return result;
        }

        private void Update()
        {
            CurrentState.Update();           
        }

        private void FixedUpdate()
        {
            CanSeePlayer();
            if (CanSeePlayer())
            {
                Debug.Log("GameLost");
                GameManager.instance.levelController.PlayerFound();
            }
            Debug.DrawLine(transform.forward, m_vDirection * m_fMaxDetectionRange, Color.blue);

        }

        /// <summary>
        /// Sets guard to distracted or if called from reset sets off.
        /// </summary>
        /// <param name="targetLight">TargetLight which player reacts to.</param>
        /// <param name="result">Is guard set on/off.</param>
        public void Distract(LightDistraction targetLight, bool result)
        {
            if (result && !m_bMoving)
            {
                Debug.Log("Hämäytetään Valolla! ");
                TargetLight = targetLight;
                m_bDistracted = result;
            }
            else
            {
                m_bDistracted = result;
                TargetLight = null;
            }
        }
        /// <summary>
        /// Sets guard to distracted or if called from reset sets off.
        /// </summary>
        /// <param name="targetSound">TargetSound which player interacts.</param>
        /// <param name="result">Is guard set on/off.</param>
        public void Distract(SoundDistraction targetSound, bool result)
        {
            if (result && m_bMoving)
            {
                TargetSound = targetSound;
                m_bDistracted = result;
            }
            else
            {
                m_bDistracted = result;
                TargetSound = null;
            }
        }

        private AIStateBase GetStateByType(AIStateType stateType)
        {
            // Returns the first object from the list _states which State property's value
            // equals to stateType. If no object is found, returns null.
            //return _states.FirstOrDefault(state => state.State == stateType);

            foreach (AIStateBase state in _states)
            {
                if (state.State == stateType)
                {
                    return state;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if guard can see player on it's detection ranges.
        /// </summary>
        /// <returns>Returns true if player is seen, otherwise false.</returns>
        public bool CanSeePlayer()
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - transform.position;
            if (Physics.Raycast(transform.position, rayDirection, out hit, m_fMaxDetectionRange))
            {
                if ((hit.collider.gameObject.GetComponent<Player>() != null) && (hit.distance <= m_fMinDetectionRange))
                {
                    Debug.Log("Player too close guard");
                    return true;
                }
            }

            if ((Vector3.Angle(rayDirection, transform.forward)) <= m_fFieldOfView * 0.5f)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit, m_fMaxDetectionRange))
                {
                    if (hit.collider.gameObject.GetComponent<Player>() != null)
                    {
                        
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                        Debug.Log(hit);
                        return true;
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Move method to 
        /// </summary>
        public override void Move(Vector3 direction)
        {
            direction = direction.normalized;
            Vector3 position = transform.position + direction * m_fMovementSpeed * Time.deltaTime;
            transform.position = position;

        }

        /// <summary>
        /// Turn method
        /// </summary>
        public override void Turn(Vector3 target)
        {
            Vector3 direction = target - transform.position;
            direction.y = transform.position.y;
            direction = direction.normalized;
            float turnSpeedRad = Mathf.Deg2Rad * m_fTurnSpeed * Time.deltaTime;
            Vector3 rotation = Vector3.RotateTowards(transform.forward,
                direction, turnSpeedRad, 0f);
            transform.rotation = Quaternion.LookRotation(rotation, transform.up);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, LightDetectDistance);
        }
    }
}