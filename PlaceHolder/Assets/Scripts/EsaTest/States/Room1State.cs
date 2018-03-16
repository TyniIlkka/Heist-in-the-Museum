namespace ProjectThief.States
{
    public class Room1State : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "Room1";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Room1; }
        }

        public Room1State()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.Lobby);
            AddTargetState(GameStateType.Room1);
            AddTargetState(GameStateType.Room2);
        }        
    }
}