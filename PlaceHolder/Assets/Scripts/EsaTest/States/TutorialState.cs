namespace ProjectThief.States
{
    public class TutorialState : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "Tutorial";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Tutorial; }
        }

        public TutorialState()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.Lobby);
            AddTargetState(GameStateType.Tutorial);            
        }
    }
}