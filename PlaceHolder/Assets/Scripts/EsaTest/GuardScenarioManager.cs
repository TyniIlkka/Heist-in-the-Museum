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

        private GameObject guard;

        [SerializeField, Tooltip("Patrol routes")]
        private List<PathPoints> pathList;
        [SerializeField, Tooltip("Potition where guard is placed.")]
        private List<StaticPosition> staticPoints;
        

        private int m_iCurrentPhase;

        private void Awake()
        {
            m_iCurrentPhase = GameManager.instance.currentPhase;
            m_sCurrentState = GameStateController.CurrentState;
            Init();
        }

        private void Init()
        {

            int i = 0;
            int j = 0;
            int k = 0;

            Debug.Log(m_iCurrentPhase);
            if (m_sCurrentState.SceneName == "Lobby")
            {
                // TODO Lobby scenarios
                switch (m_iCurrentPhase)
                {
                    case 0:
                        i = 0; j = 1;
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[j].transform.position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();
                        guard1.enabled = true;
                        guard2.enabled = true;

                        guard1.Moving = false;
                        guard2.Moving = false;

                        guard1.CurrentDirection = staticPoints[i].CurrentDir;
                        guard2.CurrentDirection = staticPoints[j].CurrentDir;

                        break;

                    case 1:
                        i = 2; j = 0;
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard3 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard4 = guard.GetComponent<Guard>();

                        guard3.enabled = true;
                        guard4.enabled = true;

                        guard3.Moving = false;
                        guard4.Moving = true;

                        guard3.CurrentDirection = staticPoints[i].CurrentDir;
                        guard4.Path = pathList[j];

                        break;

                    case 2:
                        i = 4; j = 1;
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard5 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard6 = guard.GetComponent<Guard>();

                        guard5.enabled = true;
                        guard6.enabled = true;

                        guard5.Moving = false;
                        guard6.Moving = true;

                        guard5.CurrentDirection = staticPoints[i].CurrentDir;
                        guard6.Path = pathList[j];

                        break;

                    case 3:
                        i = 5; j = 2;
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard7 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard8 = guard.GetComponent<Guard>();

                        guard7.enabled = true;
                        guard8.enabled = true;

                        guard7.Moving = false;
                        guard8.Moving = true;

                        guard7.CurrentDirection = staticPoints[i].CurrentDir;
                        guard8.Path = pathList[j];

                        break;

                    case 4:
                        i = 7; j = 8;
                        guard = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity);
                        Guard guard9 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard10 = guard.GetComponent<Guard>();

                        guard9.enabled = true;
                        guard10.enabled = true;

                        guard9.Moving = true;
                        guard10.Moving = true;

                        guard9.Path = pathList[i];
                        guard10.Path = pathList[j];
                        break;

                    case 5:
                        i = 9; j =10; k = 8;
                        guard = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity);
                        Guard guard11 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard12 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[k].transform.position, Quaternion.identity);
                        Guard guard13 = guard.GetComponent<Guard>();

                        guard11.enabled = true;
                        guard12.enabled = true;
                        guard13.enabled = true;

                        guard11.Moving = true;
                        guard12.Moving = true;
                        guard13.Moving = false;

                        guard11.Path = pathList[i];
                        guard12.Path = pathList[j];
                        guard13.CurrentDirection = staticPoints[k].CurrentDir;
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
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();

                        guard1.enabled = true;

                        guard1.Moving = false;

                        guard1.CurrentDirection = staticPoints[i].CurrentDir;
                        break;

                    case 3:
                        i = 4;
                        guard = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();

                        guard2.enabled = true;

                        guard2.Moving = true;

                        guard2.Path = pathList[i];
                        break;

                    case 4:
                        i = 5;
                        guard = SpawnGuard(pathList[i].Waypoints[0].Position, Quaternion.identity);
                        Guard guard3 = guard.GetComponent<Guard>();

                        guard3.enabled = true;

                        guard3.Moving = true;

                        guard3.Path = pathList[i];
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
                        guard = SpawnGuard(staticPoints[i].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(pathList[j].Waypoints[0].Position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();

                        guard1.enabled = true;
                        guard2.enabled = true;

                        guard1.Moving = false;
                        guard2.Moving = true;

                        guard1.CurrentDirection = staticPoints[i].CurrentDir;
                        guard2.Path = pathList[j];
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

        private GameObject SpawnGuard(Vector3 pos, Quaternion rotation)
        {
            return Instantiate(m_oGuard, pos, rotation);
        }
    }
}