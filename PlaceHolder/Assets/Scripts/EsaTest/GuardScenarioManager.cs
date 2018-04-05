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
        [SerializeField, Tooltip("Patrol routes")]
        private List<PathPoints> pathList;
        

        private int m_iCurrentPhase;

        private void Awake()
        {
            m_iCurrentPhase = GameManager.instance.currentPhase;

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
                        break;

                    case 1:
                        break;

                    case 3:
                        break;

                    case 5:
                        break;

                    case 8:
                        break;

                    case 9:
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
                        break;

                    case 6:
                        break;

                    case 7:
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