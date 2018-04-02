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
            }
            else if (m_sCurrentState.SceneName == "Room1")
            {
                // TODO Room 1 scenarios
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