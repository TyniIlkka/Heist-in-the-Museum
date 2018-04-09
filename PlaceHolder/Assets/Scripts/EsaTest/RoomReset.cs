using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectThief.States;

namespace ProjectThief
{
    public class RoomReset : MonoBehaviour
    {
        public void ResetRoom()
        {
            GameStateBase currentState = GameStateController.CurrentState;
            if (GameManager.instance.levelController.JustCleared)
                GameManager.instance.currentPhase--;

            if (currentState.SceneName == "Lobby")
            {
                switch (GameManager.instance.currentPhase)
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
            else if (currentState.SceneName == "Room1")
            {
                switch (GameManager.instance.currentPhase)
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
            else if (currentState.SceneName == "Room2")
            {
                switch (GameManager.instance.currentPhase)
                {
                    case 4:
                        break;

                    default:
                        Debug.LogError("Not valid Phase for current Room!");
                        break;
                }
            }
        }
    }
}