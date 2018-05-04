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

        Vector3 transformForward;
        Vector3 startVec;

        #region States
        public Patrol patrol;
        public PatrolMoveTo patrolMoveTo;
        public PatrolStayAtTarget patrolStayAtTarget;
        public PatrolMoveBack patrolMoveBack;
        public Static guardStatic;
        public StaticTurnTo staticTurnTo;

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

        public GuardMover guardMover;

        #region StaticGuard
        [Header("StaticGuard")]
        [SerializeField, Range(0, 40), Tooltip("How far can sounds distract guards:")]
        private float m_fLightDetectDistance;
        [SerializeField]
        private LayerMask m_lmLightMask;

        private bool m_bDistractedLight;


        #endregion

        #region Patrol
        [Header("Patrol")]
        [SerializeField, Range(0, 40), Tooltip("How far can sounds distract guards:")]
        private float m_fSoundDetectDistance;
        
        [SerializeField]
        private LayerMask m_lmSoundMask;
        //Set Path
        [SerializeField]
        private PathPoints _path;
        //How smooth guards is going to corner.
        [SerializeField]
        private float _waypointArriveDistance;
        //Which way are we moving.
        [SerializeField]
        private Direction _direction;

        private bool m_bDistractedSound;
        
        
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
        private float m_fMinDetectionRangeWalk;
        [SerializeField, Range(0, 10), Tooltip("How close guard detect to every direction.")]
        private float m_fMinDetectionRangeSneak;

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
        public bool Moving
        {
            get { return m_bMoving; }
            set { m_bMoving = value; }
        }
        public GuardMover GuardMover
        {
            get { return guardMover; }
            set { guardMover = value; }
        }

        public Waypoint CurrentWaypoint { get; set; }

        public PathPoints Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Player Thief
        {
            get { return player; }
            set { player = value; }
        }

        public float MinDetectionRange
        {
            get
            {
                if (Thief.Speed > 2f)
                {
                    return m_fMinDetectionRangeWalk;
                }
                else if (Thief.Speed <= 2f )
                {
                    return m_fMinDetectionRangeSneak;
                }
                return 0f;
            }
        }



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
        public bool DistractedLight
        {
            get { return m_bDistractedLight; }
            set { m_bDistractedLight = value; }
        }
        public bool DistractedSound
        {
            get { return m_bDistractedSound; }
            set { m_bDistractedSound = value; }
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

        // The target this guard is trying to aim at.
        public LightDistraction TargetLight { get; set; }
        public SoundDistraction TargetSound { get; set; }
        #endregion
        #endregion

        public override void Init()
        {
            Debug.Log("Init Guard");
            // Runs the base classes implementation of the Init method.
            base.Init();
            guardMover = GetComponent<GuardMover>();
            // Initializes the state system.
            // InitStates();

            transformForward = transform.forward;
            transformForward.y += 0.5f;

            startVec = transform.position;
            startVec.y += 0.5f;
        }

        /// <summary>
        /// Init States.
        /// </summary>
        public void InitStates()
        {
            patrol = new Patrol(this, _path, _direction, _waypointArriveDistance);
            _states.Add(patrol);

            patrolMoveTo = new PatrolMoveTo(this);
            _states.Add(patrolMoveTo);

            patrolStayAtTarget = new PatrolStayAtTarget(this);
            _states.Add(patrolStayAtTarget);

            patrolMoveBack = new PatrolMoveBack(this);
            _states.Add(patrolMoveBack);

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
            if (CurrentState != null)
            {
                CurrentState.Update();
                if (CurrentState == patrol)
                {
                    Animation.SetBool("Moving", true);
                }
                else if (CurrentState == staticTurnTo || CurrentState == guardStatic)
                {
                    Animation.SetBool("Moving", false);
                }

            }                   
        }

        private void FixedUpdate()
        {
            //if (CanSeePlayer())
            //{
            //    Debug.Log("GameLost");
            //    GameManager.instance.levelController.PlayerFound();
            //}
        }

        /// <summary>
        /// Sets guard to distracted or if called from reset sets off.
        /// </summary>
        /// <param name="targetLight">TargetLight which player reacts to.</param>
        /// <param name="result">Is guard set on/off.</param>
        public void Distract(LightDistraction targetLight, bool result)
        {
            if (result && (CurrentState == staticTurnTo || CurrentState == guardStatic))
            {
                Debug.Log("Hämäytetään Valolla! ");
                TargetLight = targetLight;
                m_bDistractedLight = result;
            }
            else
            {
                m_bDistractedLight = result;
                //TargetLight = null;
            }
        }
        /// <summary>
        /// Sets guard to distracted or if called from reset sets off.
        /// </summary>
        /// <param name="targetSound">TargetSound which player interacts.</param>
        /// <param name="result">Is guard set on/off.</param>
        public void Distract(SoundDistraction targetSound, bool result)
        {
            if (result && (CurrentState == patrol || CurrentState == patrolMoveTo))
            {
                TargetSound = targetSound;
                m_bDistractedSound = result;
            }
            else
            {
                m_bDistractedSound = result;
                //TargetSound = null;
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
            //Close range detection
            float distanceToPlayer = (transform.position - Thief.transform.position).magnitude; 

            if ((distanceToPlayer <= m_fMinDetectionRangeWalk ) && (Thief.GetComponent<GridPlayer>().m_fMoveSpeed > Thief.GetComponent<GridPlayer>().m_fSneakSpeed) ||
                (distanceToPlayer <= m_fMinDetectionRangeSneak) && (Thief.GetComponent<GridPlayer>().m_fMoveSpeed <= Thief.GetComponent<GridPlayer>().m_fSneakSpeed))
            {
                return true;
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
            direction.y = 0;
            //direction.x = 0; direction.z = 0;
            direction = direction.normalized;
            float turnSpeedRad = Mathf.Deg2Rad * m_fTurnSpeed * Time.deltaTime;
            Vector3 rotation = Vector3.RotateTowards(transform.forward,
                direction, turnSpeedRad, 0f);
            transform.rotation = Quaternion.LookRotation(rotation, transform.up);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, m_fMinDetectionRangeWalk);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_fMinDetectionRangeSneak);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_fMaxDetectionRange);
            //Gizmos.DrawWireSphere(transform.position, LightDetectDistance);
        }

        public void FindPath(Vector3 start, Vector3 end)
        {

        }
    }
}