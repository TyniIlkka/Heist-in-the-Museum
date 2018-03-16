namespace ProjectThief.States
{
    public class Room2State : GameStateBase
    {
        public override string SceneName
        {
            get
            {
                return "Room2";
            }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.Room2; }
        }

        public Room2State()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.Room1);
            AddTargetState(GameStateType.Room2);
        }        
    }
}