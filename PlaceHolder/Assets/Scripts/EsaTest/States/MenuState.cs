namespace ProjectThief.States
{
    public class MenuState : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "MainMenu";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.MainMenu; }
        }

        public MenuState()
        {
            AddTargetState(GameStateType.Tutorial);
            AddTargetState(GameStateType.Lobby);
            AddTargetState(GameStateType.Vault);
            AddTargetState(GameStateType.Room1);
            AddTargetState(GameStateType.Room2);
            AddTargetState(GameStateType.Room3);
        }        
    }
}