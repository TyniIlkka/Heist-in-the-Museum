using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.States;
using ProjectThief.WaypointSystem;
using ProjectThief.PathFinding;

namespace ProjectThief
{
    public class GuardScenarioManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Current scene")]
        private GameStateBase m_sCurrentState;
        [SerializeField, Tooltip("Guard prefab")]
        private GameObject m_oGuard;
        [SerializeField]
        private Player thief;

        [SerializeField, Tooltip("Patrol routes")]
        private List<PathPoints> pathList;
        [SerializeField, Tooltip("Potition where guard is placed.")]
        private List<StaticPosition> staticPoints;  

        private int m_iCurrentPhase;

        private void Start()
        {
            m_iCurrentPhase = GameManager.instance.currentPhase;
            m_sCurrentState = GameStateController.CurrentState;
            GameObject player = GameManager.instance.player;
            thief = player.GetComponent<Player>();
            Init();
        }

        private void Init()
        {


            int i = 0;
            int j = 0;
            int k = 0;

            int s = 0;  //Staticlist null position And Pathlist null position

            //Debug.Log(m_iCurrentPhase);
            if (m_sCurrentState.SceneName == "Lobby")
            {
                // TODO Lobby scenarios
                switch (m_iCurrentPhase)
                {
                    case 0:
                        i = 1; j = 1;

                        //-----Guard Spawn Middle-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, 90f);
                

                        //-----Guard Spawn Patrol T wall-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true, 0f);

                        Debug.Log("Case0 Lobby Guards spawned");
                        break;

                    case 1:
                        i = 3; j = 2;

                        //-----Guard Spawn Near X-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s , j, false, 0f);

                        //-----Guard Spawn TutorialDoor-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, 0f);

                        //-----Guard Spawn Patrol IT walls-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true, 0f);


                        Debug.Log("Case1 Lobby Guards spawned");
                        break;

                    case 2:
                        i = 3; j = 4;

                        //-----Guard Left Door-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 180f);

                        //-----Guard Spawn Around Table-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true, 0f);

                        //-----Guard Spawn Around Table-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true, 0f);

                        Debug.Log("Case2 Lobby Guards spawned");
                        break;

                    case 3:
                        i = 5; j = 2; k = 4; 
                        //-----Guard Left Door-----//
                        SpawnGuard(staticPoints[k].transform.position, Quaternion.identity, s, k, false, 180f);

                        //-----Guard Spawn EastDoor-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.Euler(0,0,0), s, i, false, 180f);

                        //-----Guard 8 Vault to Tut-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(0,0,0), i, s, true, 0f);

                        //-----Guard Spawn Patrol IT walls-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true, 0f);


                        Debug.Log("Case3 Lobby Guards spawned");
                        break;

                    case 4:
                        i = 6; j = 7;
                        //-----Guard T and Tutorial Door Path-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true, 0f);
                        //-----Guard XI Walls Roam-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.Euler(0,0,1), j, s, true, 0f);

                        //-----Guard 11 X North-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, -45f);
                        //-----Guard 11 X North-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 135f);

                        Debug.Log("Case4 Lobby Guards spawned");
                        break;

                    case 5:
                        i = 8; j = 9; k = 7;
                        //-----Guard T and Tutorial Door Path-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true, 0);
                        //-----Guard XI Walls Roam-----//
                        SpawnGuard(pathList[k].Waypoints[0].Position, Quaternion.Euler(0,0,1), k, s, true, 0);

                        //-----Guard 11 X North-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, 0);
                        //-----Guard 11 X North-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);

                        break;

                    default:
                        Debug.Log("Not valid Phase for current Room!");
                        break;
                }

            }
            else if (m_sCurrentState.SceneName == "Room1")
            {

                Debug.Log(m_iCurrentPhase);
                // TODO Room 1 scenarios
                switch (m_iCurrentPhase)
                {
                    
                    case 1:
                        i = 9; j = 11;
                        //-----Guard Room 1 phase 1 static-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);

                        //-----Guard Room 1 phase 1 patrol-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(-1,0,0), i, s, true, 0);

                        break;

                    case 2:
                        i = 9; j = 11;
                        //-----Guard Room 1 phase 1 static-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);

                        //-----Guard Room 1 phase 1 patrol-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(-1, 0, 0), i, s, true, 0);

                        break;


                    case 3:
                        i = 10; j = 11;
                        //-----Guard Room 1 phase 3 PingPong-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(-1,0,0), i, s, true, 0);

                        //-----Guard Room 1 phase 3 Loop-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true, 0);

                        break;

                    case 4:
                        i = 12; j = 13;
                        //-----Guard Room 1 phase 4 static-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, 0);

                        //-----Guard Room 1 phase 4 static-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);

                        //-----Guard Room 1 phase 4 patrol-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(0,0,1), i, s, true, 0);

                        break;

                    case 5:
                        i = 12; j = 13;
                        //-----Guard Room 1 phase 4 static-----//
                        SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false, 0);

                        //-----Guard Room 1 phase 4 static-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);

                        //-----Guard Room 1 phase 4 patrol-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(0, 0, 1), i, s, true, 0);

                        break;

                    default:
                        Debug.Log("Not valid Phase for current Room!");
                        break;
                }

            }
            else if (m_sCurrentState.SceneName == "Room2")
            {
                // TODO Room 2 scenarios
                switch (m_iCurrentPhase)
                {
                    default:
                        i = 13; j = 14;

                        //-----Guard Room 2 East Patrol-----//
                        SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.Euler(0, 0, -1), i, s, true, 0);
                        //-----Guard Room 2 West Patrol-----//
                        SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.Euler(0, 0, -1), j, s, true, 0);

                        //-----Guard Room 2 Static-----//
                        SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, s, j, false, 0);
                        break;
                }
            }
            else
            {
                Debug.Log("ERROR: Scenarios for scene not found!");
            }
        }

        /// <summary>
        /// Spawn guard method and set variables
        /// </summary>
        /// <param name="pos">Wanted position</param>
        /// <param name="rotation">Default rotation</param>
        /// <param name="pathListPosition">path position on list, if 0 = on default</param>
        /// <param name="staticListPosition">static position on list, if 0 = on default</param>
        /// <param name="moving">is guard patrolling or not</param>
        /// <param name="baseRotation">Set default rotation</param>
        /// <returns></returns>
        private void SpawnGuard(Vector3 pos, Quaternion rotation, int pathListPosition, int staticListPosition, bool moving, float baseRotation)
        {
            pos.y += 0.1f;
            GameObject guardObject = Instantiate(m_oGuard, pos, rotation);
            Guard guard = guardObject.GetComponentInChildren<Guard>();

            if (baseRotation > 0)
                guard.transform.rotation = Quaternion.Euler(0, baseRotation, 0);

            guard.GuardMover = guard.GetComponentInChildren<GuardMover>();
            guard.Path = pathList[pathListPosition];
            guard.CurrentDirection = staticPoints[staticListPosition].CurrentDir;
            guard.CurrentWaypoint = pathList[pathListPosition].Waypoints[0];
            guard.Thief = GameManager.instance.player.GetComponent<Player>();
            guard.Moving = moving;

            guard.enabled = true;
            guard.InitStates();
        }
    }
}