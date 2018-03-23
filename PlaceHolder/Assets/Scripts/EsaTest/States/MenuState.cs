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
            AddTargetState(GameStateType.Lobby);
        }

        public override void Activate()
        {
            base.Activate();
            GameManager.instance.ResetGame();
        }
    }
}