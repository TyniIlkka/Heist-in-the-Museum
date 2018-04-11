using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.States;
using ProjectThief.WaypointSystem;

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

            int s = 8;  //Staticlist null position
            int p = 10; //Pathlist null position

            //Debug.Log(m_iCurrentPhase);
            if (m_sCurrentState.SceneName == "Lobby")
            {
                // TODO Lobby scenarios
                switch (m_iCurrentPhase)
                {
                    case 0:
                        i = 0; j = 1;

                        //-----Guard 1 Spawn and set-----//
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);

                        guard1.CurrentDirection = staticPoints[i].CurrentDir;
                        guard1.Path = pathList[10];
                        
                        guard1.InitStates();
                        guard1.Thief = thief;
                        guard1.Moving = false;

                        guard1.enabled = true;

                        //-----Guard 2 Spawn and set-----//
                        Guard guard2 = SpawnGuard(staticPoints[j].transform.position, Quaternion.identity, p, j, false);

                        Debug.Log("Case0 Lobby Guards spawned");
                        break;

                    case 1:
                        i = 2; j = 0;

                        //-----Guard 3 Spawn and set-----//
                        Guard guard3 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p , i, false);

                        //-----Guard 4 Spawn and set-----//
                        Guard guard4 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        //guard4.CurrentDirection = staticPoints[11].CurrentDir;
                        //guard4.Path = pathList[j];
                        //guard4.CurrentWaypoint = pathList[j].GetClosestWaypoint(guard4.transform.position);
                        //guard4.Moving = true;
       
                        //guard4.enabled = true;

                        Debug.Log("Case1 Lobby Guards spawned");
                        break;

                    case 2:
                        i = 4; j = 1;

                        Guard guard5 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);

                        Guard guard6 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        Debug.Log("Case2 Lobby Guards spawned");
                        break;

                    case 3:
                        i = 5; j = 2;
                        Guard guard7 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);

                        Guard guard8 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        Debug.Log("Case3 Lobby Guards spawned");
                        break;

                    case 4:
                        i = 7; j = 8;
                        Guard guard9 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);
                        Guard guard10 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);

                        Debug.Log("Case4 Lobby Guards spawned");
                        break;

                    case 5:
                        i = 9; j =10; k = 8;

                        Guard guard11 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);
                        Guard guard12 = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity, j, s, true);
                        Guard guard13 = SpawnGuard(staticPoints[k].transform.position, Quaternion.identity, p, k, false);

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
                        i = 3;
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);

                        break;

                    case 3:
                        i = 4;
                        Guard guard2 = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity, i, s, true);

                        break;

                    case 4:
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
                        Guard guard1 = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity, p, i, false);
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