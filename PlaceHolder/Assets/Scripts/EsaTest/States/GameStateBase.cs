using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ProjectThief.States
{
    public enum GameStateType
    {
        Error = -1,
        MainMenu,
        Lobby,
        Vault,
        Room1,
        Room2,
        Room3
    }

    public abstract class GameStateBase
    {
        public abstract GameStateType StateType { get; }
        public abstract string SceneName { get; }

        private List<GameStateType> _valisdTargetStates = new List<GameStateType>();

        public bool AddTargetState(GameStateType targetStateType)
        {
            bool result = false;

            if (!_valisdTargetStates.Contains(targetStateType)
                && targetStateType != GameStateType.Error)
            {
                _valisdTargetStates.Add(targetStateType);
                result = true;
            }

            return result;
        }

        public bool RemoveTargetState(GameStateType targetStateType)
        {
            return _valisdTargetStates.Remove(targetStateType);
        }

        public bool InvalidTargetState(GameStateType targetStateType)
        {
            return _valisdTargetStates.Contains(targetStateType);
        }

        public virtual void Activate()
        {
            if (SceneManager.GetActiveScene().name != SceneName)
            {
                SceneManager.LoadScene(SceneName);
            }
        }

        public virtual void Deactivate() { }

    }
}