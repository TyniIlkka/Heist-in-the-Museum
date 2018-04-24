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

                        //-----Guard 1 Spawn Middle Door-----//
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false);
                

                        //-----Guard 2 Spawn Patrol T wall-----//
                        Guard guard2 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        Debug.Log("Case0 Lobby Guards spawned");
                        break;

                    case 1:
                        i = 1; j = 2;

                        //-----Guard 3 Spawn Middle Door-----//
                        Guard guard3 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s , i, false);

                        //-----Guard 4 Spawn Patrol T wall-----//
                        Guard guard4 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        //-----Guard 5 Spawn Around Table-----//
                        Guard guard5 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);


                        Debug.Log("Case1 Lobby Guards spawned");
                        break;

                    case 2:
                        i = 2; j = 2;

                        //-----Guard 6 Left Door-----//
                        Guard guard6 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false);

                        //-----Guard 7 Spawn Around Table-----//
                        Guard guard7 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        Debug.Log("Case2 Lobby Guards spawned");
                        break;

                    case 3:
                        i = 3;
                        //-----Guard 8 Tetris-----//
                        Guard guard8 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        Debug.Log("Case3 Lobby Guards spawned");
                        break;

                    case 4:
                        i = 4; j = 5; k = 3;
                        //-----Guard 9 I shape-----//
                        Guard guard9 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);
                        //-----Guard 10 L PingPong-----//
                        Guard guard10 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);
                        //-----Guard 11 X North-----//
                        Guard guard11 = SpawnGuard(staticPoints[k].transform.position, Quaternion.identity, s, k, false);

                        Debug.Log("Case4 Lobby Guards spawned");
                        break;

                    case 5:
                        i = 5; j = 6; k = 4;
                        //-----Guard 12 S PingPong-----//
                        Guard guard12 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);
                        //-----Guard 13-----//
                        Guard guard13 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        //-----Guard 14 Near Vault-----//
                        Guard guard14 = SpawnGuard(staticPoints[k].transform.position, Quaternion.identity, s, k, false);

                        break;

                    default:
                        Debug.LogError("Not valid Phase for current Room!");
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
                        i = 5;
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);

                        break;

                    case 7:
                        i = 4;
                        Guard guard2 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        break;

                    case 8:
                        i = 5;
                        Guard guard3 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        break;

                    default:
                        Debug.LogError("Not valid Phase for current Room!");
                        break;
                }

            }
            else if (m_sCurrentState.SceneName == "Room2")
            {
                // TODO Room 2 scenarios
                switch (m_iCurrentPhase)
                {
                    case 2:
                        i = 5; j = 2;
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, s, i, false);
                        Guard guard2 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        break;

                    default:
                        Debug.LogError("Not valid Phase for current Room!");
                        break;
                }
            }
            else
            {
                Debug.LogError("ERROR: Scenarios for scene not found!");
            }
        }

        private Guard SpawnGuard(Vector3 pos, Quaternion rotation, int pathListPosition, int staticListPosition, bool moving)
        {
            GameObject guardObject = Instantiate(m_oGuard, pos, rotation);
            Guard guard = guardObject.GetComponent<Guard>();
            guard.GuardMover = guard.GetComponent<GuardMover>();
            guard.Path = pathList[pathListPosition];
            guard.CurrentDirection = staticPoints[staticListPosition].CurrentDir;
            guard.CurrentWaypoint = pathList[pathListPosition].Waypoints[0];
            guard.Thief = GameManager.instance.player.GetComponent<Player>();
            guard.Moving = moving;

            guard.enabled = true;
            guard.InitStates();

            return guard;
        }
    }
}