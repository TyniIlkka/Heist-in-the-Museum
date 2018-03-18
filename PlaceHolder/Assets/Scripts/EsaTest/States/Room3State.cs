namespace ProjectThief.States
{
    public class Room3State : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "Room3";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Room3; }
        }

        public Room3State()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.Lobby);
            AddTargetState(GameStateType.Room3);
        }        
    }
}