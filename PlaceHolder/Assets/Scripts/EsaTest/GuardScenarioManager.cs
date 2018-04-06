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
        private List<GameObject> staticPoints;
        

        private int m_iCurrentPhase;

        private void Awake()
        {
            m_iCurrentPhase = GameManager.instance.currentPhase;
            m_sCurrentState = GameStateController.CurrentState;
            Init();
        }

        private void Init()
        {
            if (m_sCurrentState.SceneName == "Lobby")
            {
                // TODO Lobby scenarios
                switch (m_iCurrentPhase)
                {
                    case 0:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();

                        guard1.Moving = false;
                        guard2.Moving = false;

                        guard1.CurrentDirection = MyDirections.South;
                        guard2.CurrentDirection = MyDirections.SouthWest;

                        break;

                    case 1:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard3 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard4 = guard.GetComponent<Guard>();

                        break;

                    case 3:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard5 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard6 = guard.GetComponent<Guard>();

                        break;

                    case 5:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard7 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard8 = guard.GetComponent<Guard>();
                        break;

                    case 8:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard9 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard10 = guard.GetComponent<Guard>();

                        break;

                    case 9:
                        guard = SpawnGuard(staticPoints[0].transform.position, Quaternion.identity);
                        Guard guard11 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard12 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard13 = guard.GetComponent<Guard>();
                        break;

                    default:
                        Debug.LogError("Not valid Phase for current Room!");
                        break;
                }

            }
            else if (m_sCurrentState.SceneName == "Room1")
            {
                // TODO Room 1 scenarios
                switch (m_iCurrentPhase)
                {
                    case 2:
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();
                        break;

                    case 6:
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();
                        break;

                    case 7:
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard3 = guard.GetComponent<Guard>();
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
                    case 4:
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard1 = guard.GetComponent<Guard>();
                        guard = SpawnGuard(staticPoints[1].transform.position, Quaternion.identity);
                        Guard guard2 = guard.GetComponent<Guard>();
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