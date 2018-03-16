namespace ProjectThief.States
{
    public class LobbyState : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "Lobby";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Lobby; }
        }

        public LobbyState()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.Lobby);            
            AddTargetState(GameStateType.Vault);
            AddTargetState(GameStateType.Room1);
            AddTargetState(GameStateType.Room3);
        }        
    }
}