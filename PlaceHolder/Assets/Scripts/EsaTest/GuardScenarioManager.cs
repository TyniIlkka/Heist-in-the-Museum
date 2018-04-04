using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.States;

namespace ProjectThief
{
    public class GuardScenarioManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Current scene")]
        private GameStateBase m_sCurrentState;
        [SerializeField, Tooltip("Guard prefab")]
        private GameObject m_oGuard;
        //[SerializeField, Tooltip("Patrol routes")]
        //private ? 
        

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
                    case 1:
                        break;

                    case 3:
                        break;

                    case 5:
                        break;

                    case 7:
                        break;

                    case 11:
                        break;

                    case 13:
                        break;

                }

            }
            else if (m_sCurrentState.SceneName == "Room1")
            {
                // TODO Room 1 scenarios
                switch (m_iCurrentPhase)
                {
                    case 4:
                        break;

                    case 8:
                        break;

                    case 10:
                        break;

                }

            }
            else if (m_sCurrentState.SceneName == "Room2")
            {
                // TODO Room 2 scenarios
                switch (m_iCurrentPhase)
                {
                    case 6:
                        break;

                }
            }
            else if (m_sCurrentState.SceneName == "Room3")
            {
                // TODO Room 3 scenarios
                switch (m_iCurrentPhase)
                {
                    case 9:
                        break;

                }
            }
            else if (m_sCurrentState.SceneName == "Vault")
            {
                // TODO Vault scenarios
                switch (m_iCurrentPhase)
                {
                    case 2:
                        break;

                    case 12:
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